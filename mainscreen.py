import kivy
from kivy.lang.builder import Builder 
kivy.require("2.0.0") 
from kivy.app import App
from kivy.uix.floatlayout import FloatLayout
from kivy.core.window import Window
from kivy.lang import Builder
from kivy.clock import Clock
  
class FloatLayout(FloatLayout):
     pass

class Concussion_Detector(App):
    def build(self):
        return FloatLayout()
    
    Window.clearcolor = (1, 1, 1, 1) # White
  
# run the App
if __name__ == "__main__":
    Concussion_Detector().run()