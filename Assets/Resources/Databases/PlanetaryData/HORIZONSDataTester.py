from telnetlib import Telnet
import io
import datetime
import re
import xml.etree.cElementTree as ET

def test2():
    root = ET.Element("root")
    orbitRoot = ET.Element("root")
    #bodyList = [b'199',b'299',b'399',b'301',b'499',b'401',b'402',b'501',b'502',b'503',b'504',b'599',b'699',b'799',b'899']
    bodyList = [b'199',b'299',b'399',b'301',b'499',b'401',b'402',b'501',b'502',b'503',b'504',b'505',b'506',b'507',b'508',b'509',b'510',b'599',b'699',b'799',b'899']
    #bodyList = [b'199',b'299',b'399',b'301',b'499',b'401',b'402',b'599',b'699',b'799',b'899']
    now = datetime.datetime.now()
    print("Accessing JPL Horizons via Telnet")
    tn = Telnet('ssd.jpl.nasa.gov', 6775)
    tn.read_until(b"Horizons> ")
    for i in range(len(bodyList)):
        tn.write(bodyList[i] + b"\n")
        tn.read_until(b'Revised: ')
        planetInfo = tn.read_until(b'**')
        tn.read_until(b'<cr>: ')
        planetName = XMLPlanetData(planetInfo.decode(), root)
        tn.write(b"E\n")
        tn.read_until(b'[o,e,v,?] : ')
        tn.write(b"v\n")
        if i == 0:
            tn.read_until(b'[ <id>,coord,geo  ] : ')
            tn.write(b"500@10\n")
            tn.read_until(b'--> ')
        else:
            tn.read_until(b'[ cr=(y), n, ? ] : ')
        tn.write(b"y\n")
        tn.read_until(b'[eclip, frame, body ] : ')
        tn.write(b"eclip\n")
        tn.read_until(b'] : ')
        tn.write('{}-{}-{} {}:00\n'.format(now.year,now.month,now.day,now.hour).encode())
        tn.read_until(b'] : ')
        tn.write('{}-{}-{} {}:00\n'.format(now.year,now.month,now.day,now.hour+5).encode())
        tn.read_until(b'? ] : ')
        tn.write(b"1h\n")
        tn.read_until(b'?] : ')
        tn.write(b"y\n")
        tn.read_until(b'$$SOE')
        output = tn.read_until(b'$$EOE')

        print("Got return value from JPL Horizons for value " +bodyList[i].decode())

        buffer = io.StringIO(output.decode())
        out2 = ""
        for line in buffer:
            if line != '$$EOE':
                out2 += line
        buffer.close()
        writeDataToXML(out2, planetName, orbitRoot)

        tn.read_until(b'[R]edisplay, ? : ')
        tn.write(b"N\n")
        tn.read_until(b"Horizons> ")
    tn.write(b"exit\n")

    tn.close()

    tree = ET.ElementTree(root)
    tree.write("filename.xml")
    tree2 = ET.ElementTree(orbitRoot)
    tree2.write("orbitData.xml")

    print("End")

def writeDataToXML(data, planetName, root):
    planet = ET.SubElement(root, 'planet')
    planet.set("name",planetName)
    buffer = io.StringIO(data)

    tempNum = 1
    for line in buffer:
        if len(line.split()) == 0 or line[1] == "L":
            m = 1
        elif line[1] == 'X':
            counter = 0
            for value in line.split():
                temp = re.sub("[^0-9.E\-]", "", value)
                if temp != "":
                    if counter == 0:
                        ET.SubElement(thisTimeStep, 'X').text = temp
                        counter+= 1
                    elif counter == 1:
                        ET.SubElement(thisTimeStep, 'Y').text = temp
                        counter+=1
                    elif counter == 2:
                        ET.SubElement(thisTimeStep, 'Z').text = temp
                        counter+=1
        elif line[1] == 'V':
            counter = 0
            for value in line.split():
                temp = re.sub("[^0-9.E\-]", "", value)
                if temp != "":
                    if counter == 0:
                        ET.SubElement(thisTimeStep, 'VX').text = temp
                        counter+= 1
                    elif counter == 1:
                        ET.SubElement(thisTimeStep, 'VY').text = temp
                        counter+=1
                    elif counter == 2:
                        ET.SubElement(thisTimeStep, 'VZ').text = temp
                        counter+=1
        else:
            thisTimeStep = ET.SubElement(planet, 'dataPoint')
            thisTimeStep.set('timeStamp', line.split()[3] + " " + line.split()[4])
    
    
def XMLPlanetData(data, root):
    planet = ET.SubElement(root, 'planet')
    data = data.split()
    planetName = data[3]
    valNum = -1
    for value in data:
        valNum += 1
        if "radius" in value and 'Mean' in data[valNum -1]:
            radius = data[valNum + 3]
        elif "radius" in value and "Equat." in data[valNum -1]:
            radius = data[valNum + 4]
        elif "Radius" in value and "(IAU)" in data[valNum+1]:
            radius = data[valNum + 4]
        elif "Mass" in value and "ratio" not in data[valNum + 1] and "layers" not in data[valNum + 1]:
            if planetName == "Earth":
                mass1 = data[valNum+4]
                mass2 = data[valNum+1][3:]
            elif "Density" in data[valNum+5] or planetName == "Moon":
                mass1 = data[valNum+4]
                mass2 = data[valNum+1][4:]
            else:
                mass1 = data[valNum+5]
                mass2 = data[valNum+1][4:]
    planet.set("name",planetName)
    if "radius" in locals():
        for i in range(len(radius)):
            if radius[i] == "+" or radius[i] == "(":
                radius = radius[:i]
                break
        ET.SubElement(planet, 'radius').text = radius
    for i in range(len(mass1)):
        if mass1[i] == "+" or mass1[i] == "(":
            mass1 = mass1[:i]
            break
    ET.SubElement(planet, 'mass').text = mass1 + "E+" + mass2
    return planetName

test2()
