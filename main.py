import pygame
from sys import exit
import math
import random
from Code.player import Player
from Code.laser import Laser, BossLaser
from Code.enemy import Enemy, Boss, Chain
from Code.ui import Button, HealthBar, Message, DrawImage
from Code.loot import Loot
from os import path


class Game():
    def __init__(self,screen):
        self.screen = screen
        self.state = 'intro'
        self.phase = 'normal'
        self.boss_spawned = False
        self.boss_killed = True
        self.music = True
        self.music_playing = False
        self.scroll = 0
        self.bonus_level_timer = 0
        self.music_timer = 0

        self.spacebar_state = [False, False]
        self.shooting = False
        self.shooting_timer = 0

        self.enemy_shot_timer = 0

        self.boss_class = Boss(screen,'boss1',SCALE,FPS,BOSS_VALUE)
        self.boss_timer = 0

    def play_music(self, type):
        if self.music == True and self.music_playing == False:
            self.music_playing = True
            pygame.mixer.music.load(f'Assets/Sounds/music_{type}.mp3')
            pygame.mixer.music.set_volume(0.3)
            pygame.mixer.music.play(-1)

    def reset_game(self):
        pygame.time.set_timer(enemies_timer,500)
        # player reset
        player_class.score = 0
        player_class.health = PLAYER_HEALTH
        player_class.damage = PLAYER_DAMAGE
        player_class.double_laser = False
        player_class.shield = False
        player_class.rect.centerx = PLAYER_X
        player_class.rect.centery = PLAYER_Y
        # groups reset
        enemies_group.empty()
        loot_group.empty()
        lasers_group.empty()
        enemies_lasers_group.empty()

        self.bonus_level_timer = 0
        self.phase = 'normal'
        self.boss_class = Boss(screen,'boss1',SCALE,FPS,BOSS_VALUE)

    def draw_background(self):
        self.scroll += 1.5
        speed = 1
        for image_index in range(len(bg_images)):
            image = bg_images[image_index]

            for i in range(tiles[image_index]):
                self.screen.blit(image,(0,-i*bg_height+self.scroll*speed))
            if image_index != 0:
                speed += 0.5
        if self.scroll > 2*bg_height:
            self.scroll = 0
    
    def display_score(self):
        score_surf = font_small.render(f'Score: {player_class.score}',False,(255,255,255))
        score_rect = score_surf.get_rect(bottomleft = (10,790))
        self.screen.blit(score_surf,score_rect)

    def spawn_enemy(self):
        if player_class.score < 30: max=0.33
        elif player_class.score < 60: max=0.66
        elif player_class.score < 100: max=1
        else: max = 1

        random_number = random.uniform(0,max)

        if random_number <= 0.33: enemy_type = 'enemy1'
        elif random_number <= 0.66: enemy_type = 'enemy2'
        elif random_number <= 1: enemy_type = 'enemy3'

        if self.phase == 'normal':
            if random.random() >= ENTANGLED_ENEMIES_RATE:
                enemies_group.add(Enemy(self.screen,enemy_type,SCALE))
            else:
                enemy1 = Enemy(self.screen,enemy_type,SCALE)
                enemy2 = Enemy(self.screen,enemy_type,SCALE,enemy1.rect.centerx)
                enemy1.entangled_with = enemy2
                enemy2.entangled_with = enemy1
                enemy1.speed *= 0.5 
                enemy2.speed *= 0.5 
                enemies_group.add(Chain(self.screen,enemy1.rect,enemy2.rect,enemy1.speed,SCALE,enemy1,enemy2))
                enemies_group.add(enemy1)
                enemies_group.add(enemy2)
                
        if self.phase == 'boss' and self.boss_spawned == False:
            self.boss_spawned = True
            enemy_type = 'boss1'
            enemies_group.add(self.boss_class)
        if self.phase == 'bonus' and self.bonus_level_timer <= BONUS_LEVEL_MAX_TIME-250:
            enemies_group.add(Enemy(screen,'coin',SCALE))

    def continous_shooting(self):
        # laser shooting
        if player_class.laser_can_shoot and self.phase != 'before_boss':
            if self.spacebar_state[0] == False and self.spacebar_state[1]:
                self.shooting = True
                if player_class.double_laser == False:
                    lasers_group.add(Laser(player_class.rect.centerx,player_class.rect.centery,player_class.damage/10*SCALE+SCALE,'player',player_class.damage))
                if player_class.double_laser == True:
                    lasers_group.add(Laser(player_class.rect.centerx-player_class.rect.width/2,player_class.rect.centery+player_class.rect.height/2,player_class.damage/10*SCALE+SCALE,'player',player_class.damage))
                    lasers_group.add(Laser(player_class.rect.centerx+player_class.rect.width/2,player_class.rect.centery+player_class.rect.height/2,player_class.damage/10*SCALE+SCALE,'player',player_class.damage))
                self.spacebar_state[0] = True
            if self.shooting:  self.shooting_timer += 1
            if self.spacebar_state[0] and self.spacebar_state[1] and self.shooting_timer == SHOT_RATE:
                if player_class.double_laser == False:
                    lasers_group.add(Laser(player_class.rect.centerx,player_class.rect.centery,player_class.damage/10*SCALE+SCALE,'player',player_class.damage))
                if player_class.double_laser == True:
                    lasers_group.add(Laser(player_class.rect.centerx-player_class.rect.width/2,player_class.rect.centery+player_class.rect.height/2,player_class.damage/10*SCALE+SCALE,'player',player_class.damage))
                    lasers_group.add(Laser(player_class.rect.centerx+ player_class.rect.width/2,player_class.rect.centery+player_class.rect.height/2,player_class.damage/10*SCALE+SCALE,'player',player_class.damage))
                self.shooting_timer = 0

    def enemies_shooting(self):
        if player_class.score >= 60:
            for enemy in enemies_group:
                if enemy.type == 'enemy3':
                    if self.enemy_shot_timer == SHOT_RATE*8:
                        enemies_lasers_group.add(Laser(enemy.rect.centerx-20,enemy.rect.centery+5,SCALE/2,'enemy',enemy.laser_damage))
                        enemies_lasers_group.add(Laser(enemy.rect.centerx+20,enemy.rect.centery+5,SCALE/2,'enemy',enemy.laser_damage))
                        self.enemy_shot_timer = 0
                    self.enemy_shot_timer += 1
        if self.phase == 'boss' and self.boss_class.moving == False and self.boss_class.shooting == False and self.boss_class.spawning == False:
            enemies_lasers_group.add(BossLaser(self.screen,'boss1',self.boss_class.scale,self.boss_class.rect,self.boss_class.damage))
            self.boss_class.shooting = True

    def collision_checks(self):
        # player lasers with enemies
        for laser in lasers_group:
            enemy_hit = pygame.sprite.spritecollide(laser,enemies_group,False)
            if enemy_hit:
                for enemy in enemy_hit:
                    enemy.health -= player_class.damage
                    enemy.hit = True
                    if enemy.entangled_with != None:
                        enemy.entangled_with.health += player_class.damage
                        enemy.entangled_with.entanglement_larger()

                    if enemy.health <= 0:
                        player_class.score += enemy.value
                        enemy.death_sound.play()
                        if random.random() < enemy.drop_rate:
                            loot_group.add(Loot(SCALE-2, LOOT_SPEED, enemy.rect.center))
                laser.kill()

        # player with loot
        collected_loot = pygame.sprite.spritecollide(player.sprite,loot_group,False)
        if collected_loot:
            for loot in collected_loot:
                player_class.collect_loot(loot)
                loot.sound.play()
                loot.kill()

        # player with enemies
        enemy_hit = pygame.sprite.spritecollide(player.sprite,enemies_group,False)
        if enemy_hit:
            for enemy in enemy_hit:
                if enemy.health > 0:
                    if player_class.shield == False and enemy.type != 'coin':
                        player_class.health -= enemy.damage
                        player_class.hit = True
                        player_class.death_sound.play()
                        if enemy.type == 'chain':
                            enemy.damage = 0
                    
                    if enemy.entangled_with != None:
                        enemy.entangled_with.health += player_class.damage
                        enemy.entangled_with.entanglement_larger()

                    if enemy.type != 'chain' and enemy.type != 'boss1':
                        player_class.score += enemy.value
                        enemy.health = 0
                    if enemy.type == 'coin':
                        enemy.coin_sound.play()
                    else:
                        enemy.death_sound.play()
                    if random.random() < enemy.drop_rate:
                        loot_group.add(Loot(SCALE-2, LOOT_SPEED, enemy.rect.center))

        # player with enemies lasers
        enemy_laser_hit = pygame.sprite.spritecollide(player.sprite,enemies_lasers_group,False)
        if enemy_laser_hit:
            for laser in enemy_laser_hit:
                if player_class.shield == False:
                    player_class.health -= laser.damage
                player_class.hit = True
                player_class.death_sound.play()
                laser.kill()

    def intro(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                exit()

        self.screen.blit(starting_background, (0,0))
        self.screen.blit(game_name2, game_name_rect2)
        self.screen.blit(game_name2_2, game_name_rect2_2)
        self.screen.blit(game_name1, game_name_rect1)
        self.screen.blit(game_name1_2, game_name_rect1_2)

        if play_button.draw(): 
            self.state = 'main_game'
            self.phase = 'normal'
        
        if self.music == True:
            if music_button.draw():
                self.music = False
                self.music_playing = False
                pygame.mixer.music.fadeout(100)
        elif self.music == False:
            self.music_timer += 1
            if no_music_button.draw() and self.music_timer >= 10:
                self.music = True
                self.music_timer = 0
        self.play_music('background')

        #ranking_button.draw()
        if tutorial_button.draw():
            self.state = 'tutorial'

        self.reset_game()

    def main_game(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                exit()
            if event.type == enemies_timer:
                game_class.spawn_enemy()
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_SPACE:
                    game_class.spacebar_state = [False, True]
                if event.key == pygame.K_ESCAPE:
                    game_class.state = 'pause'
            if event.type == pygame.KEYUP:
                if event.key == pygame.K_SPACE:
                    game_class.spacebar_state = [True, False]
                    game_class.shooting = False
                    game_class.shooting_timer = 0 

        # shooting
        if self.phase != 'bonus':
            self.continous_shooting()

        # draw world
        self.draw_background()

        loot_group.draw(self.screen)
        loot_group.update()
            
        lasers_group.draw(self.screen)
        lasers_group.update()

        enemies_group.draw(self.screen)
        enemies_group.update()

        # enemies shooting lasers (enemy3, boss1)
        self.enemies_shooting()
        enemies_lasers_group.draw(self.screen)
        enemies_lasers_group.update()

        player.draw(self.screen)
        player.update()

        # check collisions
        self.collision_checks()

        # display current score
        self.display_score()

        # health bar update
        health_bar.hp = player_class.health
        health_bar.draw(self.screen)

        # game over condition
        if player_class.health <= 0: self.state = 'game_over'

        if player_class.score >= 100 and (self.phase == 'normal' or self.phase == 'before_boss'):
            self.boss_timer += 1
            self.phase = 'before_boss'
            pygame.mixer.music.fadeout(100)
            self.music_playing = False
        if self.phase == 'before_boss' and self.boss_timer == 400:
            self.boss_timer = 0
            self.phase = 'boss'
            self.play_music('boss')
            pygame.time.set_timer(enemies_timer,100)
        if self.boss_class.health <= 0 and self.phase == 'boss':
            self.phase = 'bonus'
            self.shooting = False
            if self.music: 
                pygame.mixer.music.fadeout(100)
                self.music_playing = False
        if self.phase == 'bonus':
            self.bonus_level_timer += 1
        if self.bonus_level_timer >= BONUS_LEVEL_MAX_TIME:
            self.state = 'game_won'

    def game_over(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                exit()
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_SPACE:
                    if self.state != 'main_game':
                        pygame.mixer.music.fadeout(100)
                        self.music_playing = False
                    self.state = 'main_game'
                    self.play_music('background')
                if event.key == pygame.K_ESCAPE:
                    if self.state != 'intro':
                        pygame.mixer.music.fadeout(100)
                        self.music_playing = False
                    self.state = 'intro'

        game_over_message = font.render('GAME OVER',False,(255,255,255))
        game_over_rect = game_over_message.get_rect(center = (300,300))
        self.screen.blit(game_over_message, game_over_rect)
        self.screen.blit(font_small.render('Press SPACE to play again',False,(255,255,255)), (140, 400))
        self.screen.blit(font_small.render('Press ESCAPE to go back to the main menu',False,(255,255,255)), (40, 450))

        self.reset_game()

    def pause(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                exit()
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_ESCAPE:
                    self.state = 'main_game'

        pause_message = font.render('PAUSE',False,(255,255,255))
        pause_rect = pause_message.get_rect(center = (300,400))
        self.screen.blit(pause_message, pause_rect)

    def game_won(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                exit()
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_SPACE:
                    self.state = 'main_game'
                    self.reset_game()
                    self.play_music('background')
                if event.key == pygame.K_ESCAPE:
                    self.state = 'intro'
                    
        
        won_message = font.render('YOU WON',False,(255,255,255))
        won_rect = won_message.get_rect(center = (300,300))
        self.screen.blit(won_message, won_rect)

        score_message = font.render(f'Your score is {player_class.score}',False,(255,255,255))
        score_rect = score_message.get_rect(center = (300,400))
        self.screen.blit(score_message, score_rect)

        self.screen.blit(font_small.render('Press SPACE to play again',False,(255,255,255)), (140, 500))
        self.screen.blit(font_small.render('Press ESCAPE to go back to the main menu',False,(255,255,255)), (40, 550))

    def tutorial(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                exit()

        self.screen.blit(starting_background, (0,0))

        Message(screen, font, 'TUTORIAL', 200, 80)

        Message(screen, font_small, 'Moving', 50, 140)
        Message(screen, font_little, '- left, right, up and down', 50, 160)
        DrawImage(screen, 'Assets/Miscelanous/arrow_up.png', SCALE, 480, 130)
        DrawImage(screen, 'Assets/Miscelanous/arrow_down.png', SCALE, 480, 165)
        DrawImage(screen, 'Assets/Miscelanous/arrow_left.png', SCALE, 440, 160)
        DrawImage(screen, 'Assets/Miscelanous/arrow_right.png', SCALE, 520, 160)

        Message(screen, font_small, 'Shooting', 50, 210)
        Message(screen, font_little, '- press or hold', 50, 230)
        DrawImage(screen, 'Assets/Miscelanous/spacebar_button.png', SCALE*1.5, 370, 220)
        Message(screen, font_little, 'spacebar', 330, 210)
        
        Message(screen, font_small, 'Quantum', 50, 280)
        Message(screen, font_small, 'Superposition', 50, 300)
        Message(screen, font_little, '- doubles your lasers', 50, 320)
        DrawImage(screen, 'Assets/Miscelanous/extra_laser.png', SCALE*1.2, 400, 300)

        Message(screen, font_small, 'Decoherence', 50, 370)
        Message(screen, font_small, 'Shield', 50, 390)
        Message(screen, font_little, '- protects you from damage', 50, 410)
        DrawImage(screen, 'Assets/Miscelanous/extra_shield.png', SCALE*1.2, 440, 390)

        Message(screen, font_small, 'Energy', 50, 460)
        Message(screen, font_small, 'Quant', 50, 480)
        Message(screen, font_little, '- increases your laser damage', 50, 500)
        DrawImage(screen, 'Assets/Miscelanous/extra_damage.png', SCALE*1.2, 470, 480)

        Message(screen, font_small, 'Wavefunction', 50, 550)
        Message(screen, font_small, 'Reviver', 50, 570)
        Message(screen, font_little, '- restores your health', 50, 590)
        DrawImage(screen, 'Assets/Miscelanous/extra_life.png', SCALE*1.2, 400, 570)

        Message(screen, font_small, 'Quantum', 50, 640)
        Message(screen, font_small, 'Entangled', 50, 660)
        Message(screen, font_small, 'Enemies', 50, 680)
        Message(screen, font_little, '- when one is attacked,', 50, 700)
        Message(screen, font_little, '  second gains more health', 50, 720)
        DrawImage(screen, 'Assets/Miscelanous/entangled_enemies.png', 2, 440, 660)

        if back_button.draw():
            self.state = 'intro'

    def state_manager(self):
        if self.state == 'intro': self.intro()
        if self.state == 'tutorial': self.tutorial()
        if self.state == 'main_game': self.main_game()
        if self.state == 'game_over': self.game_over()
        if self.state == 'pause': self.pause()
        if self.state == 'game_won': self.game_won()


SCREEN_WIDTH = 600
SCREEN_HEIGHT = 800
FPS = 60
LOOT_SPEED = 1
SCALE = 6
SHOT_RATE = 20
PLAYER_HEALTH = 10
PLAYER_DAMAGE = 2 ######################################################
PLAYER_X = 300
PLAYER_Y = 700
SHIELD_TIME = 10 * FPS # seconds
DOUBLE_LASER_TIME = 20 * FPS # seconds
MUSIC = True
BOSS_VALUE = 50
BONUS_LEVEL_MAX_TIME = 1000 # 10 seconds i think
ENTANGLED_ENEMIES_RATE = 0.1     

# initialize the pygame
pygame.init()
clock = pygame.time.Clock()

# set the window size
screen = pygame.display.set_mode((SCREEN_WIDTH,SCREEN_HEIGHT))

# load fonts
title_font = pygame.font.Font('Assets/Fonts/DungeonChunkMono.ttf', 110)
font = pygame.font.Font('Assets/Fonts/PixelCode.otf',40)
font_small = pygame.font.Font('Assets/Fonts/PixelCode.otf',20)
font_little = pygame.font.Font('Assets/Fonts/PixelCode.otf',15)

# set window caption
pygame.display.set_caption('Quantum Shooter')
icon = pygame.image.load('Assets/Miscelanous/icon.png')
pygame.display.set_icon(icon)

# create game class
game_class = Game(screen)

# load music
pygame.mixer.init()


# create groups
player = pygame.sprite.GroupSingle()
player_class = Player(screen, PLAYER_X, PLAYER_Y, SCALE, PLAYER_HEALTH, PLAYER_DAMAGE, SHIELD_TIME, DOUBLE_LASER_TIME)
player.add(player_class)

lasers_group = pygame.sprite.Group()

enemies_group = pygame.sprite.Group()
enemies_lasers_group = pygame.sprite.Group()

loot_group = pygame.sprite.Group()

# load images
bg_images = []
for i in range(1,4):
    bg_image = pygame.image.load(f'Assets/Backgrounds/background{i}.png').convert_alpha()
    new_width = (int(SCREEN_WIDTH / bg_image.get_width())+1)*bg_image.get_width()
    new_height = (int(SCREEN_HEIGHT / bg_image.get_height())+1)*bg_image.get_height()
    bg_image = pygame.transform.scale(bg_image, (new_width,new_height))
    bg_images.append(bg_image)
bg_height = bg_images[0].get_height()

# define game variables
tiles_number = math.ceil((SCREEN_HEIGHT/bg_height)) + 2
tiles = [tiles_number,tiles_number,tiles_number+1]
scroll = 0


# Starting screen
starting_background = pygame.image.load('Assets/Miscelanous/starting_screen.png')
starting_background = pygame.transform.scale(starting_background, (600,800))

game_name1 = title_font.render('QUANTUM',False,(255,255,255))
game_name_rect1 = game_name1.get_rect(center = (300,150))
game_name1_2 = title_font.render('SHOOTER',False,(255,255,255))
game_name_rect1_2 = game_name1.get_rect(center = (305,220))
game_name2 = title_font.render('QUANTUM',False,(180,180,180))
game_name_rect2 = game_name2.get_rect(center = (300,155))
game_name2_2 = title_font.render('SHOOTER',False,(180,180,180))
game_name_rect2_2 = game_name2.get_rect(center = (305,225))

# Buttons
play_button_image = pygame.image.load('Assets/Miscelanous/button1.png')
play_button = Button(screen, 300, 400, play_button_image, 1)

music_button_image = pygame.image.load('Assets/Miscelanous/music_button.png')
music_button = Button(screen, 200, 600, music_button_image, 1)
music_button.clicked = True

no_music_button_image = pygame.image.load('Assets/Miscelanous/no_music_button.png')
no_music_button = Button(screen, 200, 600, no_music_button_image, 1)

tutorial_button_image = pygame.image.load('Assets/Miscelanous/tutorial_button.png')
tutorial_button = Button(screen, 400, 600, tutorial_button_image, 1)

#ranking_button_image = pygame.image.load('Assets/Miscelanous/ranking_button.png')
#ranking_button = Button(screen, 460, 600, ranking_button_image, 1)

back_button_image = pygame.image.load('Assets/Miscelanous/back_button.png')
back_button = Button(screen, 60, 60, back_button_image, 4)

health_bar = HealthBar(595, 795, 250, 12, 10)

# Timer
enemies_timer = pygame.USEREVENT + 1
pygame.time.set_timer(enemies_timer,500)

while True:
    game_class.state_manager()
        
    pygame.display.update()
    clock.tick(FPS)