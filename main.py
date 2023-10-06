import pygame
from pygame.locals import K_UP, K_DOWN, K_LEFT, K_RIGHT, K_ESCAPE, KEYDOWN, QUIT, RLEACCEL, K_SPACE
from qiskit.visualization import plot_bloch_vector
import matplotlib.pyplot as plt
import io
import numpy as np


pygame.init()

SCREEN_WIDTH = 1920
SCREEN_HEIGHT = 1080

TILE_X_SIZE = 150
TILE_Y_SIZE = 150
NUMBER_OF_TILES_X = int(np.floor(SCREEN_WIDTH/TILE_X_SIZE))
NUMBER_OF_TILES_Y = int(np.floor(SCREEN_WIDTH/TILE_Y_SIZE))

grid = np.zeros((NUMBER_OF_TILES_Y,NUMBER_OF_TILES_X), dtype=int)


screen = pygame.display.set_mode((SCREEN_WIDTH, SCREEN_HEIGHT))#, pygame.RESIZABLE)
pygame.display.set_caption('Gra')

def generate_grid():
    grid = np.zeros((NUMBER_OF_TILES_Y,NUMBER_OF_TILES_X), dtype=int)
    for i in range(grid.shape[0]):
        for j in range(grid.shape[1]):
            if((i==0 or j==0) or (i==(grid.shape[0]-1) or j==(grid.shape[1]-1))):
                grid[i,j] = 1
    return(grid)

class Player(pygame.sprite.Sprite):
    def __init__(self):
        super(Player, self).__init__()
        #self.surf = pygame.Surface((75,25))
        #self.surf.fill((255,255,255))
        self.surf = pygame.image.load('imgs/hero/hero.png')
        self.rect = self.surf.get_rect()

        
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

class Bomb():
    def __init__(self):
        self.time = 10000
        self.quantum_state = np.array([1,0])

    def decrease_timer(self):
        self.time -= 1

    def update_state(self, matrix):
        self.quantum_state = np.dot(matrix,self.quantum_state)
        # self.quantum_state = [self.quantum_state[0] + coords[0], self.quantum_state[1] + coords[1]]

    def measurement(self):
        # probability = np.power(np.sin(self.quantum_state[0]/2),2)
        probability = np.power(np.abs(self.quantum_state[0]),2)
        measurement_result = int(np.random.binomial(1, probability))
        return(measurement_result)

class Qubit(pygame.sprite.Sprite):
    def __init__(self):
        super(Qubit, self).__init__()
        self.coords = [0,0]
        self.last_update = 0
        self.surf = None
        self.rect = None
        self.reLoadImage()

    def reLoadImage(self):
        buffer = io.BytesIO()
        plot_bloch_vector([1]+self.coords, coord_type='spherical', figsize=(2,2))
        plt.savefig(buffer, format="png")
        plt.close()
        buffer.seek(0)
        self.surf = pygame.image.load(buffer).convert()
        self.rect = self.surf.get_rect(center=(500,500))


class Gate(pygame.sprite.Sprite):
    def __init__(self, gate_type, center):
        super(Gate, self).__init__()
        self.gate_type = gate_type
        self.matrix = None
        if(gate_type == 'H'):
            self.surf = pygame.image.load('imgs/gates/H.png')
            self.matrix = 1/np.sqrt(2)*np.array([[1,1],[1,-1]])
        elif(gate_type == 'X'):
            self.surf = pygame.image.load('imgs/gates/X.png')
            self.matrix = np.array([[0,1],[1,0]])
        self.rect = self.surf.get_rect(center=center)

class Wall(pygame.sprite.Sprite):
    def __init__(self):
        super(Wall, self).__init__()
        self.surf = pygame.Surface((75,75))
        self.surf.fill((255,255,0))
        self.rect = self.surf.get_rect(center=(400,400))

def state_to_coords(state):
    theta = 2*np.arccos(state[0])
    phi = np.angle(state[1])
    return [theta, phi]

player = Player()
qubit = Qubit()
H = Gate('H', (200,200))
X = Gate('X',(350,200))

H_boom = Gate('H', (500,200))
X_boom = Gate('X',(650,200))

bomb = Bomb()

wall = Wall()

all_sprites = pygame.sprite.Group()
gates = pygame.sprite.Group()
walls = pygame.sprite.Group()
all_sprites.add(player)
all_sprites.add(qubit)
all_sprites.add(H)
all_sprites.add(X)
all_sprites.add(wall)
gates.add(H)
gates.add(X)
walls.add(wall)

clock = pygame.time.Clock()

running = True

while running:

    for event in pygame.event.get():
        if event.type == QUIT:
            running = False

        elif event.type == KEYDOWN:
            if event.key == K_ESCAPE:
                running = False
            if event.key == K_SPACE:
                if bomb.measurement():
                    all_sprites.add(H_boom)
                    bomb = Bomb()
                else:
                    all_sprites.add(X_boom)


    pressed_keys = pygame.key.get_pressed()

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

    screen.fill((0,0,0))
    for entity in all_sprites:
        screen.blit(entity.surf, entity.rect)

    gate_pass = pygame.sprite.spritecollideany(player, gates)
    if gate_pass:
        bomb.update_state(gate_pass.matrix)
        qubit.coords = state_to_coords(bomb.quantum_state)
        qubit.reLoadImage()
        gate_pass.kill()

    #wall_collision = pygame.sprite.spritecollideany(player, walls)
    #if(wall_collision):
    #    player.rect.center = old_pos

    pygame.display.flip()

    clock.tick(50)
    

pygame.quit()