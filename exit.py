import pygame

class Exit(pygame.sprite.Sprite):
    def __init__(self, topleft, TILE_SIZE):
        super(Exit, self).__init__()
        self.surf = pygame.Surface((TILE_SIZE,TILE_SIZE))
        self.surf.fill((255,0,0))
        self.rect = self.surf.get_rect(topleft=topleft)