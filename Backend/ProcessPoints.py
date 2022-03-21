import csv

class ProcessPoints():

    def __init__(self):
        self.ReadCSV()
        self.AnalysePoints()
    
    def ReadCSV(self):
        x = []
        y = []

        with open('./files/eye-coordinatesCSV.csv', 'r') as f:
            reader = csv.reader(f, delimiter = ',')
            for row in reader:
                x.append(int(row[0]))
                y.append(int(row[1]))
            self.x = x
            self.y = y

    def AnalysePoints(self):
        minX = min(self.x)
        minY = min(self.y)
        maxX = max(self.x)
        maxY = max(self.y)

        print(minX)
        print(minY)
        print(maxX)
        print(maxY)

        midX = (maxX + minX) / 2
        midY = (maxY + minY) / 2

        print("The midpoint of {maxx} & {minx} = {midx}".format(maxx=maxX, minx=minX,midx=midX))
        #print(midX)
        #print(midY)

