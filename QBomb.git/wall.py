import pygame

class Wall(pygame.sprite.Sprite):
    def __init__(self, topleft, TILE_SIZE):
        super(Wall, self).__init__()
        self.surf = pygame.image.load('imgs/surroundings/wall.png')
        self.rect = self.surf.get_rect(topleft = topleft)

class BlankWall(pygame.sprite.Sprite):
    def __init__(self, topleft, TILE_SIZE):
        super(BlankWall, self).__init__()
        self.surf = pygame.image.load('imgs/surroundings/blank_tile.png')
        self.rect = self.surf.get_rect(topleft = topleft)