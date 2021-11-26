import kivy 
kivy.require("2.0.0") 
from kivy.app import App
from kivy.uix.floatlayout import FloatLayout
from kivy.core.window import Window
  
  
# class in which we are creating the canvas
class FloatLayout(FloatLayout):
    pass
  
# Create the App Class
class Concussion_Detector(App):
    def build(self):
        Window.clearcolor = (1, 1, 1, 1) # White
        return FloatLayout()
  
# run the App
if __name__ == "__main__":
    Concussion_Detector().run()