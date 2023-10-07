import pygame

class Player(pygame.sprite.Sprite):
    def __init__(self, screen,  x, y, scale, health, damage, max_shield_time, max_double_laser_time):
        super().__init__()
        self.screen = screen
        self.x = x
        self.y = y

        player_fly_center = pygame.image.load('Assets/Ships/Player/ship1.png').convert_alpha()
        new_size = (player_fly_center.get_width()*scale, player_fly_center.get_height()*scale)
        player_fly_center = pygame.transform.scale(player_fly_center, new_size)

        player_fly_right = pygame.image.load('Assets/Ships/Player/ship1_right.png').convert_alpha()
        new_size = (player_fly_right.get_width()*scale, player_fly_right.get_height()*scale)
        player_fly_right = pygame.transform.scale(player_fly_right, new_size)

        player_fly_left = pygame.image.load('Assets/Ships/Player/ship1_left.png').convert_alpha()
        new_size = (player_fly_left.get_width()*scale, player_fly_left.get_height()*scale)
        player_fly_left = pygame.transform.scale(player_fly_left, new_size)
        
        flames = []
        for i in range(1,5):
            flame = pygame.image.load(f'Assets/Miscelanous/flame{i}.png')
            flame = pygame.transform.scale_by(flame, scale)
            flames.append(flame)
        self.flames = flames
        self.flames_animation_index = 0

        self.shields = []
        for i in range(1,5):
            shield = pygame.image.load(f'Assets/Miscelanous/shield{i}.png')
            shield = pygame.transform.scale_by(shield, scale)
            self.shields.append(shield)
        self.shields_animation_index = 0
        self.shields_animation_index2 = 0
        self.shield_timer = 0
        self.double_laser_timer = 0
        self.hit_animation_index = 0
        self.death_animation_index = 0

        self.player_fly = [player_fly_center,player_fly_left,player_fly_right]
        self.player_index = 0
        self.vertical_move = 0
        self.player_speed = 5
        self.double_laser = False
        self.health = health
        self.maxhealth = health
        self.shield = False
        self.max_shield_time = max_shield_time
        self.max_double_laser_time = max_double_laser_time
        self.damage = damage
        self.hit = False

        self.image = self.player_fly[self.player_index]
        self.rect = self.image.get_rect(center = (x, y))

        self.laser_can_shoot = True

        self.score = 0

        self.explosions = []
        for i in range(2,5):
            explosion = pygame.image.load(f'Assets/Miscelanous/explosion{i}.png').convert_alpha()
            explosion = pygame.transform.scale_by(explosion, scale*3)
            explosion_rect = explosion.get_rect(center = (self.rect.centerx, self.rect.centery))
            self.explosions.append([explosion,explosion_rect])

        self.death_sound = pygame.mixer.Sound('Assets/Sounds/audio_explosion.wav')
        self.death_sound.set_volume(0.1)

    def health_check(self):
        if self.health > self.maxhealth:
            self.health = self.maxhealth

    def player_input(self):
        keys = pygame.key.get_pressed()
        if keys[pygame.K_LEFT]: self.player_index = 1
        if keys[pygame.K_RIGHT]: self.player_index = 2
        if keys[pygame.K_UP]: self.vertical_move = 1
        if keys[pygame.K_DOWN]: self.vertical_move = 2

    def animation(self):
        # movement: center, left or right
        self.image = self.player_fly[self.player_index]
        
        # flames
        self.flames_animation_index += 0.3
        if self.flames_animation_index >= len(self.flames): self.flames_animation_index = 0
        if self.player_index == 0:
            self.screen.blit(self.flames[int(self.flames_animation_index)], (self.rect.x+18,self.rect.y+42))
        elif self.player_index == 1:
            self.screen.blit(self.flames[int(self.flames_animation_index)], (self.rect.x+18-6,self.rect.y+42))
        elif self.player_index == 2:
            self.screen.blit(self.flames[int(self.flames_animation_index)], (self.rect.x+18-6,self.rect.y+42))

        # shield
        if self.shield:
            # shield appearing
            self.shields_animation_index += 0.05
            if self.shields_animation_index >= len(self.shields): self.shields_animation_index = len(self.shields)-1
            if self.shield_timer < self.max_shield_time - 60:
                self.screen.blit(self.shields[int(self.shields_animation_index)], (self.rect.x-self.rect.width/2,self.rect.y-self.rect.height/2))
            # shield disappearing
            if self.shield_timer >= self.max_shield_time - 60 and self.shield_timer < self.max_shield_time:
                self.shields_animation_index2 += 4
                if self.shields_animation_index2 >= 20:
                    self.screen.blit(self.shields[-1], (self.rect.x-self.rect.width/2,self.rect.y-self.rect.height/2))
                if self.shields_animation_index2 == 20 * 2: self.shields_animation_index2 = 0
            self.shield_timer += 1
            # shield reset
            if self.shield_timer == self.max_shield_time:
                self.shields_animation_index = 0
                self.shield = False
                self.shield_timer = 0

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

        # death
        if self.health <= 0:
            self.death_animation_index += 0.2
            if self.death_animation_index >= len(self.explosions):
                self.kill()
                return False

    def double_laser_disabling(self):
        self.double_laser_timer += 1
        if self.double_laser_timer >= self.max_double_laser_time:
            self.double_laser_timer = 0
            self.double_laser = False

    def apply_movement(self):
        # movement horizontal
        if self.player_index == 1 and self.rect.left > 20:
            self.rect.x -= self.player_speed
        if self.player_index == 2 and self.rect.right < self.screen.get_width()-20:
            self.rect.x += self.player_speed

        # movement vertical
        if self.vertical_move == 1 and self.rect.top > 20:
            self.rect.y -= self.player_speed
        if self.vertical_move == 2 and self.rect.bottom < self.screen.get_height()-70:
            self.rect.y += self.player_speed

        # reset state
        self.player_index = 0
        self.vertical_move = 0

    def collect_loot(self, loot):
        if loot.type == 'extra_laser':
            self.double_laser = True
        elif loot.type == 'extra_life':
            self.health += 1
        elif loot.type == 'extra_shield':
            self.shield = True
        elif loot.type == 'extra_damage':
            self.damage += 1

    def update(self):
        self.player_input()
        self.animation()
        self.apply_movement()
        self.double_laser_disabling()
        self.health_check()