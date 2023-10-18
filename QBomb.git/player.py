import pygame
from pygame.locals import K_UP, K_DOWN, K_LEFT, K_RIGHT, K_ESCAPE, KEYDOWN, QUIT, RLEACCEL, K_SPACE

class Player(pygame.sprite.Sprite):
    def __init__(self, topleft):
        super(Player, self).__init__()
        #self.surf = pygame.Surface((75,25))
        #self.surf.fill((255,255,255))
        self.surf = pygame.image.load('imgs/hero/hero_new.png')
        self.rect = self.surf.get_rect(topleft = topleft)

        
    def update_vertical(self, pressed_keys):
        if pressed_keys[K_UP]:
            self.rect.move_ip(0,-5)
        if pressed_keys[K_DOWN]:
            self.rect.move_ip(0,5)

    def update_horizontal(self, pressed_keys):
        if pressed_keys[K_LEFT]:
            self.rect.move_ip(-5,0)
        if pressed_keys[K_RIGHT]:
            self.rect.move_ip(5,0)