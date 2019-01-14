from telnetlib import Telnet
import io
import datetime
import re
import xml.etree.cElementTree as ET

def test2():
    root = ET.Element("root")
    orbitRoot = ET.Element("root")
    bodyList = [b'199',b'299',b'399',b'301',b'499',b'401',b'402',b'599',b'501',b'502',b'503',b'504',b'699',b'601',b'602',b'603',b'604',b'605',b'606',b'607',b'608',b'609',b'610',b'611',b'612',b'613',b'614',b'799',b'701',b'702',b'703',b'704',b'705',b'899',b'801',b'999',b'901',b'-125544',b'-143205',b'-98']
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
        tn.write('{}-{}-{} {}:00\n'.format(now.year,now.month+1,now.day-2,now.hour).encode())
        tn.read_until(b'? ] : ')
        tn.write(b"1h\n")
        tn.read_until(b'?] : ')
        tn.write(b"y\n")
        tn.read_until(b'$$SOE')
        output = tn.read_until(b'$$EOE')

        print("Got return value from JPL Horizons for value " +planetName)

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
    if planetName == "134340":
        planetName = "Pluto"
    elif planetName == "International":
        planetName = "ISS"
    elif planetName == "Tesla":
        planetName = "Starman"
    elif planetName == "New":
        planetName = "New Horizons"
    valNum = -1
    for value in data:
        valNum += 1
        if "rot." in value and planetName != "Earth":
            if planetName != "Mars":
                rotation = data[valNum+3]
            else:
                rotation = str(float(data[valNum+3])/24)
        if "Orbit" in value and "period" in data[valNum+1] and planetName == "Moon":
            rotation = data[valNum+3]
        if "Sidereal" in value and "hr" in data[valNum+2] and planetName == "Earth":
            rotation = data[valNum+4]
        if "Sidereal" in value and data[valNum+1] == "hr" and planetName == "Earth":
            rotation = str(float(data[valNum+4])/24)
            print(rotation)
        if "radius" in value and 'Mean' in data[valNum -1]:
            radius = data[valNum + 3]
        elif "radius" in value and "Equat." in data[valNum -1]:
            radius = data[valNum + 4]
        elif "Radius" in value and "(IAU)" in data[valNum+1]:
            radius = data[valNum + 4]
        elif "Radius" in value and "Pluto" in data[valNum+2]:
            radius = data[valNum + 5]
        elif "Radius" in value and planetName == "Charon":
            radius = data[valNum + 3]
        elif "Mass" in value and "ratio" not in data[valNum + 1] and "layers" not in data[valNum + 1]:
            if planetName == "Earth":
                mass1 = data[valNum+4]
                mass2 = data[valNum+1][3:]
            elif planetName == "Pluto":
                mass1 = data[valNum+5]
                mass2 = data[valNum+2][4:]
            elif "Density" in data[valNum+5] or planetName == "Moon" or data[valNum+5] == "Geometric" or data[valNum+5] == "+-":
                mass1 = data[valNum+4]
                mass2 = data[valNum+1][4:]
            else:
                mass1 = data[valNum+5]
                mass2 = data[valNum+1][4:]
    print(planetName)
    if planetName == "ISS":
        rotation = "0"
        radius = "0.11"
        mass1 = "419455"
        mass2 = "0"
    elif planetName == "Starman":
        rotation = "0"
        radius = "0.093"
        mass1 = "6000"
        mass2 = "0"
    elif planetName == "New Horizons":
        rotation = "0"
        radius = "0.0027"
        mass1 = "465"
        mass2 = "0"
    planet.set("name",planetName)
    if "radius" in locals():
        for i in range(len(radius)):
            if radius[i] == "+" or radius[i] == "(":
                radius = radius[:i]
                break
        ET.SubElement(planet, 'radius').text = radius
    if "rotation" in locals():
        for i in range(len(rotation)):
            if rotation[i] == "+":
                rotation = rotation[:i]
                break
        ET.SubElement(planet, 'rotation').text = rotation
    for i in range(len(mass1)):
        if mass1[i] == "+" or mass1[i] == "(":
            mass1 = mass1[:i]
            break
    ET.SubElement(planet, 'mass').text = mass1 + "E+" + mass2
    return planetName

test2()
