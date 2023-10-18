import pygame
import random
from Code.laser import BossLaser


class Enemy(pygame.sprite.Sprite):
    def __init__(self, screen, type, scale, entangled_enemy_x=None):
        super().__init__()
        self.screen = screen
        self.type = type

        enemy = pygame.image.load(f'Assets/Ships/Enemies/{type}.png').convert_alpha()
        enemy = pygame.transform.scale_by(enemy, scale)
        enemy_bigger = pygame.transform.scale_by(enemy, 1.05)
        self.frames = [enemy,enemy_bigger]

        if type == 'enemy1':
            self.scale = scale
            self.value = 1
            self.health = 3
            self.damage = 1
            self.speed = 4
            self.drop_rate = 0.1
        elif type == 'enemy2':
            self.scale = scale+2
            self.value = 2
            self.health = 5
            self.damage = 2
            self.speed = 6
            self.drop_rate = 0.2
        elif type == 'enemy3':
            self.scale = scale+2
            self.value = 3
            self.health = 7
            self.damage = 3
            self.laser_damage = 1
            self.speed = 3
            self.drop_rate = 0.5
        elif type == 'coin':
            self.scale = scale
            self.value = 1
            self.health = 1
            self.damage = 0
            self.speed = 4
            self.drop_rate = 0

        self.explosions = []
        for i in range(2,5):
            explosion = pygame.image.load(f'Assets/Miscelanous/explosion{i}.png').convert_alpha()
            explosion = pygame.transform.scale_by(explosion, self.scale)
            self.explosions.append(explosion)

        self.animation_index = 0
        self.animation_index_hit = 0
        self.death_animation_index = 0
        self.image = self.frames[self.animation_index]

        # random choosing the x_position
        if entangled_enemy_x == None:
            self.rect = self.image.get_rect(center = (random.randint(50,550),-100))
        elif entangled_enemy_x != None:
            if entangled_enemy_x < self.screen.get_width()/2:
                min = entangled_enemy_x + 100
                max = self.screen.get_width() - 50
            elif entangled_enemy_x >= self.screen.get_width()/2:
                min = 50
                max = entangled_enemy_x - 100
            self.rect = self.image.get_rect(center = (random.randint(min, max),-100))

        self.hit = False
        self.max_health = self.health

        self.entangled = False
        self.entangled_with = None

        # death sound
        self.death_sound = pygame.mixer.Sound('Assets/Sounds/audio_explosion.wav')
        self.death_sound.set_volume(0.3)

        # coin collect sound
        self.coin_sound = pygame.mixer.Sound('Assets/Sounds/audio_coin.wav')
        self.coin_sound.set_volume(0.3)

    def animation(self):
        # life
        self.animation_index += 0.1
        if self.animation_index >= len(self.frames): self.animation_index = 0
        self.image = self.frames[int(self.animation_index)]
        self.rect = self.image.get_rect(center = (self.rect.centerx, self.rect.centery))

        # hit
        if self.hit:
            self.animation_index_hit += 0.1
            hit_explosion = self.explosions[0]
            hit_explosion = pygame.transform.scale_by(hit_explosion, 1/3)
            hit_explosion_rect = hit_explosion.get_rect(center = (self.rect.centerx, self.rect.centery))
            self.screen.blit(hit_explosion, hit_explosion_rect)
            #self.screen.blit(hit_explosion, (self.rect.x+7,self.rect.y+9))
            if self.animation_index_hit == 0.4:
                self.hit = False
                self.animation_index_hit = 0
            
        # death
        if self.health <= 0:
            self.death_animation_index += 0.2
            if self.death_animation_index >= len(self.explosions): 
                self.kill()
                return False
            if self.type != 'coin':
                explosion = self.explosions[int(self.death_animation_index)]
                explosion_rect = explosion.get_rect(center = (self.rect.centerx, self.rect.centery))
                self.screen.blit(explosion, explosion_rect)
                #self.screen.blit(self.explosions[int(self.death_animation_index)], (self.rect.x-3,self.rect.y-3))

    def entanglement_larger(self):
        for i in range(len(self.frames)):
            self.frames[i] = pygame.transform.scale_by(self.frames[i], 1.25)

    def destroy(self):
        if self.rect.y >= 900:
            self.kill()

    def update(self):
        self.animation()
        if self.health > 0:
            self.rect.y += self.speed
        self.destroy()


class Boss(pygame.sprite.Sprite):
    def __init__(self,screen,type,scale,FPS,BOSS_VALUE):
        super().__init__()
        self.screen = screen
        self.type = type
        self.scale = scale

        enemy = pygame.image.load(f'Assets/Ships/Enemies/{type}.png').convert_alpha()
        enemy = pygame.transform.scale_by(enemy, scale*1.5)
        self.image = enemy
        self.rect = self.image.get_rect(center = (screen.get_width()/2, -self.image.get_height()))

        self.moving = False
        self.new_moving = False
        self.spawning = True
        self.stop_timer = 0
        self.max_stop_time = FPS
        self.random_x = self.rect.centerx
        self.death_animation_index = 0
        self.hit_animation_index = 0
        self.hit = False
        self.shooting = False
        self.entangled_with = None

        if type == 'boss1':
            self.value = BOSS_VALUE
            self.health = 200
            self.damage = 4
            self.speed = 3
            self.drop_rate = 0

        self.explosions = []
        for i in range(2,5):
            explosion = pygame.image.load(f'Assets/Miscelanous/explosion{i}.png').convert_alpha()
            explosion = pygame.transform.scale_by(explosion, scale*3)
            explosion_rect = explosion.get_rect(center = (self.rect.centerx, self.rect.centery))
            self.explosions.append([explosion,explosion_rect])

        self.death_sound = pygame.mixer.Sound('Assets/Sounds/audio_explosion.wav')
        self.death_sound.set_volume(0.3)

    def apply_movement(self):
        if self.spawning:
            if self.rect.centery <= self.image.get_height():
                self.rect.centery += abs(self.speed)
            else:
                self.spawning = False
                self.moving = False
    
        if self.moving == False and self.new_moving:
            if self.rect.centerx - 100 > 75 and self.rect.centerx + 100 < self.screen.get_width() - 75:
                random_number = random.randint(0,1)
            elif self.rect.centerx - 100 > 75:
                random_number = 0
            elif self.rect.centerx + 100 < self.screen.get_width() - 75:
                random_number = 1
                
            if random_number == 0:
                x_min = 75
                x_max = self.rect.centerx - 100
                self.speed = - abs(self.speed)
            elif random_number == 1:
                x_min = self.rect.centerx + 100
                x_max = self.screen.get_width() - 75
                self.speed = abs(self.speed)

            self.random_x = random.randint(x_min, x_max)
            self.moving = True
            self.new_moving = False

        if self.moving:
            self.rect.centerx += self.speed
            self.shooting = False

        if self.spawning == False and abs(round(self.rect.centerx - self.random_x)) <= abs(self.speed*2):
            self.moving = False
            self.new_moving = False
            self.stop_timer += 1
            if self.stop_timer == self.max_stop_time:
                self.new_moving = True
                self.stop_timer = 0

    def animation(self):
        # death
        if self.health <= 0:
            self.death_animation_index += 0.2
            if self.death_animation_index >= len(self.explosions): 
                self.kill()
                return False

            self.image = pygame.transform.scale_by(self.image, 1-self.death_animation_index/15)
            self.rect = self.image.get_rect(center = (self.rect.centerx, self.rect.centery))
            explosion_image = self.explosions[int(self.death_animation_index)][0]
            explosion_rect = explosion_image.get_rect(center = (self.rect.centerx, self.rect.centery))
            self.screen.blit(self.explosions[int(self.death_animation_index)][0], explosion_rect)

        # hit
        if self.hit:
            self.hit_animation_index += 0.1
            hit_explosion = self.explosions[0][0]
            hit_explosion = pygame.transform.scale_by(hit_explosion, 1/2)
            hit_explosion_rect = hit_explosion.get_rect(center = (self.rect.centerx, self.rect.centery))
            self.screen.blit(hit_explosion, hit_explosion_rect)
            if self.hit_animation_index == 0.4:
                self.hit = False
                self.hit_animation_index = 0

    def update(self):
        self.apply_movement()
        self.animation()


class Chain(pygame.sprite.Sprite):
    def __init__(self,screen,position1,position2,speed,scale,enemy1,enemy2):
        super().__init__()
        self.screen = screen
        self.position1 = position1
        self.position2 = position2
        self.speed = speed
        self.scale = scale
        self.type = 'chain'

        self.enemy1 = enemy1
        self.enemy2 = enemy2
        self.enemy1_dead = False
        self.enemy2_dead = False
        self.entangled_with = None

        enemy = pygame.image.load(f'Assets/Miscelanous/chain.png').convert_alpha()
        enemy = pygame.transform.scale_by(enemy, scale)
        enemy = pygame.transform.scale(enemy, (abs(position1.centerx-position2.centerx),enemy.get_height()/2))

        self.image = enemy
        if position1.centerx < position2.centerx:
            self.rect = self.image.get_rect(midleft = (position1.centerx,position1.centery))
        elif position1.centerx >= position2.centerx:
            self.rect = self.image.get_rect(midleft = (position2.centerx,position2.centery))
    
        self.health = 100
        self.damage = 1
        self.value = 0
        self.drop_rate = 0

        self.death_sound = pygame.mixer.Sound('Assets/Sounds/audio_explosion.wav')
        self.death_sound.set_volume(0.1)

    def apply_movement(self):
        self.rect.y += self.speed
        if self.rect.y > self.screen.get_height() * 1.1:
            self.kill()

    def destroy(self):
        if self.enemy1.health <= 0: self.enemy1_dead = True
        if self.enemy2.health <= 0: self.enemy2_dead = True

        if self.enemy1_dead and self.enemy2_dead:
            self.kill()

    def update(self):
        self.apply_movement()
        self.destroy()
