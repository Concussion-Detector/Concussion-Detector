from kivy.app import App
from kivy.uix.gridlayout import GridLayout
from kivy.uix.image import Image
from kivy.core.window import Window
import mainscreen
from time import sleep

class SplashScreen(App):
    def build(self):
        self.window = GridLayout()
        self.window.cols = 1
        print("Building")
        Window.clearcolor = (1, 1, 1, 1)
        #add widgets to window
        self.window.add_widget(Image(source="logo.jpg"))

        return self.window


if __name__ == "__main__":
    SplashScreen().run()
