import pygame

class Wall(pygame.sprite.Sprite):
    def __init__(self, topleft):
        super(Wall, self).__init__()
        self.surf = pygame.Surface((150,150))
        self.surf.fill((255,255,0))
        self.rect = self.surf.get_rect(topleft=topleft)