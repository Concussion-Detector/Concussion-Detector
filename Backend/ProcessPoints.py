import csv

class ProcessPoints():

    def __init__(self):
        self.ReadCSV()
        self.AnalysePoints()
    
    def ReadCSV(self):
        x = []
        y = []
        points = []

        with open('./files/eye-coordinatesCSV.csv', 'r') as f:
            reader = csv.reader(f, delimiter = ',')
            for row in reader:
                x.append(int(row[0]))
                y.append(int(row[1]))
                point = (int(row[0]), int(row[1]))
                points.append(point)
            self.x = x
            self.y = y
            self.points = points

    def AnalysePoints(self):
        minX = min(self.x)
        minY = min(self.y)
        maxX = max(self.x)
        maxY = max(self.y)

        print(minX)
        print(minY)
        print(maxX)
        print(maxY)

        midX = int((maxX + minX) / 2)
        midY = int((maxY + minY) / 2)
        
        # Create an acceptable range for x and y
        lX = midX - 1
        hX = midX + 1
        lY = midY - 1
        hY = midY + 1

        goodPoints = 0
        badPoints = 0

        for point in self.points:
            if point[0] in range(lX,hX):
                goodPoints += 1
            elif point[1] in range(lY,hY):
                goodPoints += 1
            else:
                badPoints += 1
        
        print("Good points: {goodPoints}".format(goodPoints=goodPoints))
        print("Bad counts: {badPoints}".format(badPoints=badPoints))

        allPoints = goodPoints + badPoints

        concussionChance = round((badPoints / allPoints) * 100, 2)

        print("Chance of being concussed is {chance}%".format(chance=concussionChance))
            

