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
4. 


***

# Video Presentation

![YouTube](https://img.shields.io/badge/ConcussionDetector-%23FF0000.svg?style=for-the-badge&logo=YouTube&logoColor=white)




