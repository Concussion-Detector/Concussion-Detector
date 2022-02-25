from pymongo import MongoClient


class Database(object):

    """
    This class provides all the database relatead operations
    """

    def __init__(self):
        client = MongoClient(
            'mongodb+srv://concussion-detector:gmitproject2022@cluster.rure1.mongodb.net/myFirstDatabase?retryWrites=true&w=majority'
        )
        # Root of database
        db = client.concussionDB
        # Baseline data
        colBaseline = db.baseline
        # Concussion data
        colConcussionTests = db.concussionTests
    
    

    #writeToFileCSV = open("./files/eye-coordinatesCSV.csv", "w")

    def save_to_file(x, y, fileName):
        fileName.write(str(x) + ", " + str(y) + "\n")

    