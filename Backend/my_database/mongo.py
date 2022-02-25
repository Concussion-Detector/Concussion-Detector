from pymongo import MongoClient

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
    
    def SaveToDatabase(self, option, id, coords):
        test = {
            "user_id": id,
            "coords": coords
        }
        
        if option == 1:
            self.db.colBaseline.insert_one(test)
        elif option == 2:
            self.db.colBaseline.insert_one(test)

        

    

    