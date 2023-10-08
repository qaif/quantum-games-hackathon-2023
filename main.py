import pygame
import pygame_menu
from pygame.locals import K_UP, K_DOWN, K_LEFT, K_RIGHT, K_ESCAPE, KEYDOWN, QUIT
from qiskit.visualization import plot_bloch_vector
import matplotlib.pyplot as plt
import io
import numpy as np

from wall import Wall
from grid import generate_grid, grid_2_walls, grid_2_exit, grid_2_player, grid_2_gates, grid_2_floors, grid_2_start, generate_lvl1, generate_lvl2, generate_lvl3, generate_lvl4, generate_lvl5, generate_lvl6
from player import Player
from bomb import Bomb, Timer, GUI_timer
from qubit import Qubit
from gate import Gate
from gui import Envelope

pygame.init()

SCREEN_WIDTH = 1200
SCREEN_HEIGHT = 700

TILE_SIZE = 100
NUMBER_OF_TILES_X = int(np.floor(SCREEN_HEIGHT/TILE_SIZE))
NUMBER_OF_TILES_Y = int(np.ceil(SCREEN_WIDTH/TILE_SIZE))


screen = pygame.display.set_mode((SCREEN_WIDTH, SCREEN_HEIGHT))#, pygame.RESIZABLE)
pygame.display.set_caption('QBomb')

class Explosion(pygame.sprite.Sprite):
    def __init__(self):
        super(Explosion, self).__init__()
        self.surf = pygame.image.load('imgs/explosion.png').convert()
        self.rect = self.surf.get_rect(center=(SCREEN_WIDTH//2, SCREEN_HEIGHT//2))

class Win(pygame.sprite.Sprite):
    def __init__(self):
        super(Win, self).__init__()
        self.surf = pygame.image.load('imgs/win.png').convert().convert_alpha()
        self.rect = self.surf.get_rect(center=(SCREEN_WIDTH//2, SCREEN_HEIGHT//2))


class TimeOut(pygame.sprite.Sprite):
    def __init__(self):
        super(TimeOut, self).__init__()
        self.surf = pygame.image.load('imgs/clock_small.png').convert()
        self.rect = self.surf.get_rect(center=(300, SCREEN_HEIGHT//2))


def state_to_coords(state):
    theta = np.real_if_close(2*np.arccos(state[0]))
    phi = np.real_if_close(np.angle(state[1]))
    return [theta, phi]


#def paused():
#    paused=True
#    while paused:
#        for event in pygame.event.get():
#            if event.key == K_ESCAPE:
#                    paused = False
#    pygame.display.update()

def game_main_loop(lvl=1):

    if(lvl==1):
        grid = generate_lvl1()
    if(lvl==2):
        grid = generate_lvl2()
    if(lvl==3):
        grid = generate_lvl3()
    if(lvl==4):
        grid = generate_lvl4()
    if(lvl==5):
        grid = generate_lvl5()
    if(lvl==6):
        grid = generate_lvl6()
    #grid = generate_grid(NUMBER_OF_TILES_X=NUMBER_OF_TILES_X, NUMBER_OF_TILES_Y=NUMBER_OF_TILES_Y)
    start = grid_2_start(grid, TILE_SIZE=TILE_SIZE)
    walls = grid_2_walls(grid, TILE_SIZE=TILE_SIZE)
    player = grid_2_player(grid, TILE_SIZE=TILE_SIZE)
    exit = grid_2_exit(grid, TILE_SIZE=TILE_SIZE)
    gates = grid_2_gates(grid, TILE_SIZE=TILE_SIZE)
    floors = grid_2_floors(grid, TILE_SIZE=TILE_SIZE)

    gui_pos = (grid.shape[0]*TILE_SIZE,0)
    qubit_pos = (gui_pos[0]-40, gui_pos[1]+35)
    qubit = Qubit(qubit_pos)
    gui = Envelope(gui_pos)
    timer_pos = (gui_pos[0]-2*TILE_SIZE-5, gui_pos[1]+245)
    timer = Timer(timer_pos, TILE_SIZE)
    #H = Gate('H', (400,200))
    #X = Gate('X',(350,200))
    #RZ = Gate('RZ2',(700,200))

    # H_boom = Gate('H', (500,200))
    # X_boom = Gate('X',(650,200))
    explosion = Explosion()
    win = Win()
    timeout_obj = TimeOut()

    bomb = Bomb()

    # All sprites
    all_sprites = pygame.sprite.Group()
    all_sprites.add(start)
    for f in floors:
        all_sprites.add(f)
    for w in walls:
        all_sprites.add(w)
    all_sprites.add(exit)
    for g in gates:
        all_sprites.add(g)
    all_sprites.add(player)
    all_sprites.add(timer)
    all_sprites.add(gui)
    all_sprites.add(qubit)
    # /All sprites



    clock = pygame.time.Clock()

    running = True
    key_pressed = False

    exploded = False
    winner = False

    while running:

        for event in pygame.event.get():
            if event.type == QUIT:
                running = False

            elif event.type == KEYDOWN:
                key_pressed = True
                if event.key == K_ESCAPE:
                    running = False


        pressed_keys = pygame.key.get_pressed()


        #---------------
        old_pos = player.rect.center
        player.update_horizontal(pressed_keys)
        wall_collision_horizontal = pygame.sprite.spritecollideany(player, walls)
        if(wall_collision_horizontal):
            player.rect.center = old_pos

        old_pos = player.rect.center
        player.update_vertical(pressed_keys)
        wall_collision_vertical = pygame.sprite.spritecollideany(player, walls)
        if(wall_collision_vertical):
            player.rect.center = old_pos

        #---------------
        # exit_collision = pygame.sprite.collide_rect(player, exit)
        # if(exit_collision): win = True
        #---------------
        
        #---------------
        if(key_pressed): 
            bomb.decrease_timer()
            timer.update(bomb.get_time_ratio())
        #---------------
        
        screen.fill((0,0,0))
        for entity in all_sprites:
            screen.blit(entity.surf, entity.rect)

        gate_pass = pygame.sprite.spritecollideany(player, gates)
        if gate_pass:
            if gate_pass.gate_type == 'M':
                if bomb.measurement():
                    exploded = True
                    bomb.quantum_state = np.array([1,0])
                else:
                    bomb.quantum_state = np.array([0,1])
                    bomb.time = bomb.max_time
            else:
                bomb.update_state(gate_pass.matrix)

            qubit.coords = state_to_coords(bomb.quantum_state)
            qubit.reLoadImage()
            gate_pass.kill()

        if pygame.sprite.collide_rect(player, exit):
            if bomb.measurement():
                exploded = True
                bomb.quantum_state = np.array([1,0])
            else:
                bomb.quantum_state = np.array([0,1])
                winner = True

            qubit.coords = state_to_coords(bomb.quantum_state)
            qubit.reLoadImage()
            exit.kill()
           
        #-------------
        if exploded:
            screen.fill((0,0,0))
            screen.blit(explosion.surf, explosion.rect)
        if winner:
            screen.blit(win.surf, win.rect)
            
            #paused()
        if(bomb.time == 0):
            #screen.blit(timeout_obj.surf, timeout_obj.rect)
            if bomb.measurement():
                exploded = True
                bomb.quantum_state = np.array([1,0])
                screen.fill((0,0,0))
                screen.blit(explosion.surf, explosion.rect)
            else:
                bomb.quantum_state = np.array([0,1])
                bomb.time = bomb.max_time
           
            qubit.coords = state_to_coords(bomb.quantum_state)
            qubit.reLoadImage()

        pygame.display.flip()

        if(winner):
            pygame.time.wait(2000)
            running = False
        if exploded:
            pygame.time.wait(2000)
            running = False

        clock.tick(50)

def level_selection():
    lvl_menu = pygame_menu.Menu('Welcome',SCREEN_WIDTH, SCREEN_HEIGHT, theme=pygame_menu.themes.THEME_BLUE)
    lvl_menu.add.button('Level 1', lambda: game_main_loop(1))
    lvl_menu.add.button('Level 2', lambda: game_main_loop(2))
    lvl_menu.add.button('Level 3', lambda: game_main_loop(3))
    lvl_menu.add.button('Level 4', lambda: game_main_loop(4))
    lvl_menu.add.button('Level 5', lambda: game_main_loop(5))
    lvl_menu.add.button('Level 6', lambda: game_main_loop(6))
    lvl_menu.add.button('Quit', pygame_menu.events.EXIT)
    lvl_menu.mainloop(screen)

menu = pygame_menu.Menu('Welcome',SCREEN_WIDTH, SCREEN_HEIGHT, theme=pygame_menu.themes.THEME_BLUE)

#menu.add.button('Play', game_main_loop)
menu.add.button('Play', level_selection)
menu.add.button('Quit', pygame_menu.events.EXIT)

menu.mainloop(screen)

pygame.quit()