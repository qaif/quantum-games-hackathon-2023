import pygame

class Wall(pygame.sprite.Sprite):
    def __init__(self, topleft, TILE_SIZE):
        super(Wall, self).__init__()
        self.surf = pygame.Surface((TILE_SIZE,TILE_SIZE))
        self.surf.fill((255,255,0))
        self.rect = self.surf.get_rect(topleft=topleft)