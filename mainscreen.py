import kivy
from kivy.lang.builder import Builder 
kivy.require("2.0.0") 
from kivy.app import App
from kivy.uix.floatlayout import FloatLayout
from kivy.uix.gridlayout import GridLayout
from kivy.core.window import Window
from kivy.graphics import Color
from kivy.graphics import Ellipse
from kivy.animation import Animation
from kivy.uix.widget import Widget
from kivy.lang import Builder
from kivy.clock import Clock
  
#Builder.load_file('Concussion_Detector.kv')

# class in which we are creating the canvas
# class FloatLayout(FloatLayout):
#     def animate_it(self, *args):
#         animate = Animation(

#         )
#     pass
  
# # Create the App Class
# class Concussion_Detector(App):
#     def build(self):
#         Window.clearcolor = (1, 1, 1, 1) # White
#         return FloatLayout()
  
# # run the App
# if __name__ == "__main__":
#     Concussion_Detector().run()

class MyGrid(FloatLayout):

    def __init__(self, **kwargs):
        super(MyGrid, self).__init__(**kwargs)
        self.inside = GridLayout()
        self.inside.cols = 12

        self.add_widget(self.inside)

        # POSITIONS
        self.positionLeft_1 = 50
        self.positionLeft_2 = 150
        self.positionLeft_3 = 250

        with self.canvas:
            Color(1, 1, 1, 1, mode='rgba')
            self.line_Left = Ellipse(pos=(self.center_x - 15/2, self.center_y - 15/2), size=(15, 15))

            animate_left_line_01 = Animation(pos=(self.positionLeft_1, 215), t='out_circ')
            animate_left_line_02 = Animation(pos=(self.positionLeft_2, 215), t='out_circ')
            animate_left_line_03 = Animation(pos=(self.positionLeft_3, 215), t='out_circ')

        # START ANIMATION
        Clock.schedule_once(self.anim1, 0)
        Clock.schedule_once(self.anim2, 3)
        Clock.schedule_once(self.anim3, 6)

    def anim1(self, df):
        # MOVE TO POSITION 1
        animate_left_line_01 = Animation(pos=(self.positionLeft_1, 215), t='out_circ')
        animate_left_line_01.start(self.line_Left)

    def anim2(self, df):
        # MOVE TO POSITION 2
        animate_left_line_02 = Animation(pos=(self.positionLeft_2, 215), t='out_circ')
        animate_left_line_02.start(self.line_Left)

    def anim3(self, df):
        # MOVE TO POSITION 3
        animate_left_line_03 = Animation(pos=(self.positionLeft_3, 215), t='out_circ')
        animate_left_line_03.start(self.line_Left)

    def move(self):
        pass

    def update(self, dt):
        self.move()

class Concussion_Detector(App):
    def build(self):
        self.game = MyGrid()
        Clock.schedule_interval(self.game.update, 1.0 / 60.0)
        return self.game()
    
    Window.clearcolor = (1, 1, 1, 1) # White
  
# run the App
if __name__ == "__main__":
    Concussion_Detector().run()