import pygame

class Envelope(pygame.sprite.Sprite):
    def __init__(self, topright):
        super(Envelope, self).__init__()
        self.surf = pygame.image.load('imgs/GUI/GUI_rescaled.png').convert_alpha()
        self.rect = self.surf.get_rect(topright = topright)