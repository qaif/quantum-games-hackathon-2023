import pygame
import random

class Loot(pygame.sprite.Sprite):
    def __init__(self, scale, speed, center_position):
        super().__init__()
        extra_laser = pygame.image.load('Assets/Miscelanous/extra_laser.png').convert_alpha()
        extra_laser = pygame.transform.scale_by(extra_laser,scale)
        extra_life = pygame.image.load('Assets/Miscelanous/extra_life.png').convert_alpha()
        extra_life = pygame.transform.scale_by(extra_life,scale)
        extra_shield = pygame.image.load('Assets/Miscelanous/extra_shield.png').convert_alpha()
        extra_shield = pygame.transform.scale_by(extra_shield,scale)
        extra_damage = pygame.image.load('Assets/Miscelanous/extra_damage.png').convert_alpha()
        extra_damage = pygame.transform.scale_by(extra_damage,scale)
        self.loot = [extra_laser,extra_life,extra_shield,extra_damage]

        type = random.randint(0,3)
        if type == 0: self.type = 'extra_laser'
        if type == 1: self.type = 'extra_life'
        if type == 2: self.type = 'extra_shield'
        if type == 3: self.type = 'extra_damage'

        self.image = self.loot[type]
        self.rect = self.image.get_rect(center = center_position)

        self.speed = speed

        self.sound = pygame.mixer.Sound('Assets/Sounds/audio_coin.wav')
        self.sound.set_volume(0.3)

    def destroy(self):
        if self.rect.y >= 900:
            self.kill()

    def update(self):
        self.rect.y += 1
        self.destroy()