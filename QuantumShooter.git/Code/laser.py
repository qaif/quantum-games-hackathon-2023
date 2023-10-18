import pygame

class Laser(pygame.sprite.Sprite):
    def __init__(self,x,y,scale,type,damage):
        super().__init__()
        self.type = type

        if self.type == 'player':
            self.image = pygame.image.load('Assets/Missiles/missile1.png').convert_alpha()
            self.image = pygame.transform.scale_by(self.image,scale)
            self.rect = self.image.get_rect(midbottom = (x,y))

            # sound
            laser_sound = pygame.mixer.Sound('Assets/Sounds/audio_laser.wav')
            laser_sound.set_volume(0.5)
            laser_sound.play()

        elif self.type == 'enemy':
            self.image = pygame.image.load('Assets/Missiles/missile2.png').convert_alpha()
            self.image = pygame.transform.rotate(self.image, 45)
            self.image = pygame.transform.scale_by(self.image,scale)
            self.rect = self.image.get_rect(midbottom = (x,y))
            self.damage = damage

    def apply_movement(self):
        if self.type == 'player':
            self.rect.y -= 8
            if self.rect.y < -10:
                self.kill()

        if self.type == 'enemy':
            self.rect.y += 8
            if self.rect.y > 850:
                self.kill()

    def update(self):
        self.apply_movement()


class BossLaser(pygame.sprite.Sprite):
    def __init__(self,screen,type,scale,position,damage):
        super().__init__()
        self.screen = screen
        self.scale = scale
        self.position = position
        self.damage = damage
        self.timer = 0
        self.timer_death = 0
        self.lasers = []
        for i in range(1,4):
            laser = pygame.image.load(f'Assets/Miscelanous/{type}_laser{i}.png').convert_alpha()
            laser = pygame.transform.rotate(laser, -90)
            laser = pygame.transform.scale_by(laser, self.scale)
            self.lasers.append(laser)

        self.image = pygame.transform.scale(self.lasers[2], (self.lasers[2].get_width()/4,self.lasers[2].get_height()))
        self.rect = self.image.get_rect(midtop = (self.position.centerx, self.position.bottom-20))
    
    def apply_movement(self):
        self.timer += 0.4

        self.image = pygame.transform.scale(self.lasers[2], (self.lasers[2].get_width()/4,self.lasers[2].get_height()*int(self.timer)))
        self.rect = self.image.get_rect(midtop = (self.position.centerx, self.position.bottom-20))

        image = self.lasers[0]
        rect = image.get_rect(midtop = (self.position.centerx, self.position.bottom-20))
        self.screen.blit(image, rect)

        laser_number = int((self.screen.get_height()-self.position.height-rect.bottom)/rect.height)

        if self.timer >= 1:
            if self.timer < laser_number + 1: max = int(self.timer)
            else: max = laser_number + 1

            for i in range(max):
                image = self.lasers[1]
                rect = image.get_rect(midtop = (self.position.centerx, self.position.bottom+rect.height*i))
                self.screen.blit(image, rect)

        if self.timer >= laser_number + 1:
            image = self.lasers[2]
            rect = image.get_rect(midtop = (self.position.centerx, self.position.bottom+rect.height*(laser_number+1)))
            self.screen.blit(image, rect)
        if self.timer >= laser_number + 2:
            self.timer = laser_number + 2
            self.timer_death += 1
            if self.timer_death >= 20:
                self.kill()

    def update(self):
        self.apply_movement()


        