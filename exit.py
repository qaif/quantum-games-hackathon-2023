import pygame

class Exit(pygame.sprite.Sprite):
    def __init__(self, topleft, TILE_SIZE):
        super(Exit, self).__init__()
        self.surf = pygame.image.load('imgs/surroundings/finish_tile.png')
        self.rect = self.surf.get_rect(topleft = topleft)

class Start(pygame.sprite.Sprite):
    def __init__(self, topleft, TILE_SIZE):
        super(Start, self).__init__()
        self.surf = pygame.image.load('imgs/surroundings/start_tile.png')
        self.rect = self.surf.get_rect(topleft = topleft)