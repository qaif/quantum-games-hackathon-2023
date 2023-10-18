import pygame

class Floor(pygame.sprite.Sprite):
    def __init__(self, topleft, TILE_SIZE):
        super(Floor, self).__init__()
        self.surf = pygame.image.load('imgs/surroundings/floor2.png')
        self.rect = self.surf.get_rect(topleft = topleft)