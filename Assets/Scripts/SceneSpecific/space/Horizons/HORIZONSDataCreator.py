#!/usr/bin/env python
from telnetlib import Telnet
import io
import datetime
import re
import xml.etree.cElementTree as ET

def test2():
    root = ET.Element("root")
    orbitRoot = ET.Element("root")
    bodyList = [b'10',b'301']
    now = datetime.datetime.now()
    print("Accessing JPL Horizons via Telnet")
    tn = Telnet('ssd.jpl.nasa.gov', 6775)
    tn.read_until(b"Horizons> ")
    
    for i in range(len(bodyList)):
        tn.write(bodyList[i] + b"\n")
        if(bodyList[i] == b'10'):
            tn.read_until(b'Revised : ')
        else:
            tn.read_until(b'Revised: ')
        planetInfo = tn.read_until(b'**')
        tn.read_until(b'<cr>: ')
        planetName = XMLPlanetData(planetInfo.decode(), root)
        tn.write(b"E\n")
        tn.read_until(b'[o,e,v,?] : ')
        tn.write(b"v\n")
        if i == 0:
            tn.read_until(b'[ <id>,coord,geo  ] : ')
            tn.write(b"@399\n")
        else:
            tn.read_until(b'[ cr=(y), n, ? ] : ')
        tn.write(b"y\n")
        tn.read_until(b'[eclip, frame, body ] : ')
        tn.write(b"eclip\n")
        tn.read_until(b'] : ')
        tn.write('{}-{}-{} {}:00\n'.format(now.year,now.month,now.day,now.hour).encode())
        tn.read_until(b'] : ')
        tn.write('{}-{}-{} {}:00\n'.format(now.year,now.month+3,now.day,now.hour).encode())
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
                print(temp*30)
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
    return planetName

test2()
