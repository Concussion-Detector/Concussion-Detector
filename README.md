<p align="center">
  <img src="https://user-images.githubusercontent.com/57759154/140659027-396b5850-35dd-408e-8a57-51adbcfd9bdc.png" />
</p>

***

# Concussion-Detector

| **Project Title** | Concussion Detector
| :------------- |:-------------|
| **Course**              | BSc (Hons) in Software Development |
| **Module**              | Applied Project and Minor Dissertation |
| **Institute**           | [Galway-Mayo Institute of Technology](https://www.gmit.ie/) |
| **Students**             | [Patrick Murray](https://github.com/PatrickMurray78) - G00344530@gmit.ie <br> [Wojciech Pogorzelski](https://github.com/wojtekpogo) - G00375250@gmit.ie |
| **Project Supervisor**     | Damien Costello |
| **Module Supervisor**   | Damien Costello |

***

## Table of Content
- [About the Project](#About-the-project)
- [Repository Contents](#Repository-contents)
- [Technologies used](#Technologies-used)
- [How to Install](#How-to-install)
- [How to Run](#How-to-run)
- [Video Demonstration](#Video-demonstation)
- [References](#References)

***

## About the Project
Our goal was to create an application which will roughly determine if an individual suffers with concussion.

The following objectives where identified to achieve our goals:

**Obtain Data** - Using a video from the camera, obtain player's pupil movement and extract x,y coordinates.

**Process Data** - Processing the captured data in order to create a graphical interpretation of x,y coordinates from both eyes.

**Compare Data** - Comparing both baseline and post concussion data. Once compared there should be a visible deviation in the pattern as the eye movement will be altered when player is concussed.

**Save Data** - Various data such as player's info, baseline and post concussion states are securely stored which allows to use each players data for further examination. 

**Report Findings** -  Provides a visual feedback of the results of the assessment. 

![roadmap](https://user-images.githubusercontent.com/55446533/159567244-cf8ebe9a-9aa1-44f5-8bea-8ca526ca15f7.jpg)

***

# Technologies

* ![Python](https://img.shields.io/badge/python-3670A0?style=for-the-badge&logo=python&logoColor=ffdd54) - *version: 3.8.8*
* ![MongoDB](https://img.shields.io/badge/MongoDB-%234ea94b.svg?style=for-the-badge&logo=mongodb&logoColor=white) - *version: 4.0.1*
* ![Unity](https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white) - *version: 2020.3.29f1*
* ![OpenCV](https://img.shields.io/badge/opencv-%23white.svg?style=for-the-badge&logo=opencv&logoColor=white) - *version: 4.3.3*
* ![NumPy](https://img.shields.io/badge/numpy-%23013243.svg?style=for-the-badge&logo=numpy&logoColor=white) - *version: 1.20.1*
* ![Firebase](https://img.shields.io/badge/firebase-%23039BE5.svg?style=for-the-badge&logo=firebase)
* ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
* ![GitHub](https://img.shields.io/badge/github-%23121011.svg?style=for-the-badge&logo=github&logoColor=white)

***

# How To Run

### Backend

1. Make sure you have a **Python** installed. Run `python --version` in your command prompt to verify. If not then click [here](https://www.python.org/downloads/)
2. Run `pip install -r requirements.txt` - this will install all required libraries. *Note - you may need to use `pip3` instead of `pip`*
3. Run `python main.py` 
4. Your eye detector is now up and running.

### Frontend

1. Simply run `ConcussionDetector.exe` file.
2. Your application is now up and running.


***

# User Guide

1. After setting up the application, make sure you are properly alligned and get into comfortable position. 
2. Optimal distance away from the camera is 40cm (16inches). 
3. The viewing angle has to be consistent as well. The user should be looking directly at the camera.


When the application launches, you will be presented with the following screen:

![image](https://user-images.githubusercontent.com/48318312/168925218-8afac4de-5aa8-4aa4-bcf5-05cacdcc4e50.png)

You can start off by clicking the start button which will allow you to record a test or you can click the settings
icon in the top right to view reports for a specified patient or exit the application.

### Start

By default you will record baseline data when a full name is entered, however you may select the post-concussion checkbox to 
record post-concussion data but that patient needs to have baseline data in the database.

![image](https://user-images.githubusercontent.com/48318312/168926067-d3b7a9fc-e338-442d-b1a8-b040133aed7c.png)

Click 'Go' to begin.

### Follow the Dot

Assuming the patient is alligned and in a comfortable position, they may begin following the dot around the scnree.

![image](https://user-images.githubusercontent.com/48318312/168927297-242308f7-76e5-4fa5-8354-8b899d5a8d5d.png)

Once complete, you will be redirected to the reports page. If you recorded a baseline tests, there will only be one chart but if you
recorded a post-concussion test, then there will be two charts to allow for comparison.

### Reports

If a baseline tests was recorded, there will only be one chart on dislay.
If a post-concussion test was recorded, you will be presented with two charts to allow for comparison.

![image](https://user-images.githubusercontent.com/48318312/168927564-07e4d30f-88fa-46ad-a487-0c840152cfb5.png)

You can exit back to the main screen at any point by clicking the back icon in the top right.

### Search

When at the main screen, click the settings icon in the top right. This will open a dropdown in which you can select
"Search Patient". Once this option is selected, a popup will appear in which you can enter the first and last name
of the patient you wish to search. 

![image](https://user-images.githubusercontent.com/48318312/168928017-76c4b749-4d3a-4422-adb3-f51294c2c9fc.png)

If data was found, you will be redirected to the reports page where you can view the data.

***

# Video Presentation

![YouTube](https://img.shields.io/badge/ConcussionDetector-%23FF0000.svg?style=for-the-badge&logo=YouTube&logoColor=white)




