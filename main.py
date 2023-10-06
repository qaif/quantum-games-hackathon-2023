import pygame
from pygame.locals import K_UP, K_DOWN, K_LEFT, K_RIGHT, K_ESCAPE, KEYDOWN, QUIT, RLEACCEL, K_SPACE
from qiskit.visualization import plot_bloch_vector
import matplotlib.pyplot as plt
import io
import numpy as np

from wall import Wall
from grid import generate_grid, grid_2_walls

pygame.init()

SCREEN_WIDTH = 1920
SCREEN_HEIGHT = 1080

TILE_SIZE = 100
NUMBER_OF_TILES_X = int(np.floor(SCREEN_HEIGHT/TILE_SIZE))
NUMBER_OF_TILES_Y = int(np.ceil(SCREEN_WIDTH/TILE_SIZE))


screen = pygame.display.set_mode((SCREEN_WIDTH, SCREEN_HEIGHT))#, pygame.RESIZABLE)
pygame.display.set_caption('Gra')

#def generate_grid():
#    grid = np.zeros((NUMBER_OF_TILES_Y,NUMBER_OF_TILES_X), dtype=int)
#    for i in range(grid.shape[0]):
#        for j in range(grid.shape[1]):
#            if((i==0 or j==0) or (i==(grid.shape[0]-1) or j==(grid.shape[1]-1))):
#                grid[i,j] = 1
#    return(grid)

class Player(pygame.sprite.Sprite):
    def __init__(self):
        super(Player, self).__init__()
        #self.surf = pygame.Surface((75,25))
        #self.surf.fill((255,255,255))
        self.surf = pygame.image.load('imgs/hero/hero.png')
        self.rect = self.surf.get_rect(topleft = (200,200))

        
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
        rho = np.angle(self.quantum_state[0])
        if rho != 0:
            self.quantum_state*=np.exp(-rho*1j)
        self.quantum_state = np.real_if_close(self.quantum_state)

    def measurement(self):
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
        self.rect = self.surf.get_rect(topright=(SCREEN_WIDTH,0))

gates_ids = {'H':5, 'X':6, 'RY3':7, 'RY4':8, 'RZ2':9}
gates_matrices = {
    'H':1/np.sqrt(2)*np.array([[1,1],[1,-1]]),
    'X':np.array([[0,1],[1,0]]),
    'RY3':np.array([[np.cos(np.pi/6),-np.sin(np.pi/6)],[np.sin(np.pi/6),np.cos(np.pi/6)]]),
    'RY4':np.array([[np.cos(np.pi/8),-np.sin(np.pi/8)],[np.sin(np.pi/8),np.cos(np.pi/8)]]),
    'RZ2':np.array([[1,0],[0,np.exp(np.pi/2*1j)]])
}

class Gate(pygame.sprite.Sprite):
    def __init__(self, gate_type, center):
        super(Gate, self).__init__()
        self.gate_type = gate_type
        self.id = gates_ids[gate_type]
        self.matrix = gates_matrices[gate_type]
        self.surf = pygame.image.load('imgs/gates/'+gate_type+'.png')
        self.rect = self.surf.get_rect(center=center)

def state_to_coords(state):
    theta = np.real_if_close(2*np.arccos(state[0]))
    phi = np.real_if_close(np.angle(state[1]))
    return [theta, phi]

player = Player()
qubit = Qubit()

grid = generate_grid(NUMBER_OF_TILES_X=NUMBER_OF_TILES_X, NUMBER_OF_TILES_Y=NUMBER_OF_TILES_Y)
walls = grid_2_walls(grid, TILE_SIZE=TILE_SIZE)

H = Gate('H', (400,200))
X = Gate('X',(350,200))
RZ = Gate('RZ2',(700,200))

H_boom = Gate('H', (500,200))
X_boom = Gate('X',(650,200))

bomb = Bomb()


all_sprites = pygame.sprite.Group()
gates = pygame.sprite.Group()
all_sprites.add(player)
all_sprites.add(H)
all_sprites.add(X)
all_sprites.add(RZ)
for w in walls:
    all_sprites.add(w)
all_sprites.add(qubit)

gates.add(H)
gates.add(X)
gates.add(RZ)


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

    screen.fill((0,0,0))
    for entity in all_sprites:
        screen.blit(entity.surf, entity.rect)

    gate_pass = pygame.sprite.spritecollideany(player, gates)
    if gate_pass:
        bomb.update_state(gate_pass.matrix)
        qubit.coords = state_to_coords(bomb.quantum_state)
        qubit.reLoadImage()
        gate_pass.kill()

    pygame.display.flip()

    clock.tick(50)
    

pygame.quit()