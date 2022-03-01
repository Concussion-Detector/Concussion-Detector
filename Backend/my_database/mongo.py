from pymongo import MongoClient
from datetime import datetime

class Database(object):

    """
    This class provides all the database relatead operations
    """

    def __init__(self):
        self.client = MongoClient(
            'mongodb+srv://concussion-detector:gmitproject2022@cluster.rure1.mongodb.net/myFirstDatabase?retryWrites=true&w=majority'
        )

        # Root of database
        self.db = self.client.concussionDB
        # Baseline data
        self.colBaseline = self.db.baseline
        # Concussion data
        self.colConcussionTests = self.db.concussionTests
    
    def SaveToDatabase(self, option, uuid, coords):

        # Create a string with today's date
        today = datetime.now()
        todays_date = today.strftime("%d/%m/%Y %H:%M")

        test = {
            "uuid": uuid,
            "coords": coords,
            "date": todays_date
        }

        if option == "1":
            self.db.colBaseline.insert_one(test)
        elif option == "2":
            self.db.colConcussionTests.insert_one(test)

        

    

    