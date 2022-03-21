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
                x.append(row[0])
                y.append(row[1])
            self.x = x
            self.y = y

    def AnalysePoints(self):
        print('x')
        print(self.x)
        print('y')
        print(self.y)

