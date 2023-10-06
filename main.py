import pygame
from pygame.locals import K_UP, K_DOWN, K_LEFT, K_RIGHT, K_ESCAPE, KEYDOWN, QUIT, RLEACCEL, K_SPACE
from qiskit.visualization import plot_bloch_vector
import matplotlib.pyplot as plt
import io
import numpy as np


pygame.init()

SCREEN_WIDTH = 1920
SCREEN_HEIGHT = 1080

screen = pygame.display.set_mode((SCREEN_WIDTH, SCREEN_HEIGHT))#, pygame.RESIZABLE)
pygame.display.set_caption('Gra')

class Player(pygame.sprite.Sprite):
    def __init__(self):
        super(Player, self).__init__()
        #self.surf = pygame.Surface((75,25))
        #self.surf.fill((255,255,255))
        self.surf = pygame.image.load('imgs/hero/hero.png')
        self.rect = self.surf.get_rect()

        
    def update(self, pressed_keys):
        if pressed_keys[K_UP]:
            self.rect.move_ip(0,-5)
        if pressed_keys[K_DOWN]:
            self.rect.move_ip(0,5)
        if pressed_keys[K_LEFT]:
            self.rect.move_ip(-5,0)
        if pressed_keys[K_RIGHT]:
            self.rect.move_ip(5,0)


        if self.rect.left < 0:
              self.rect.left = 0
        if self.rect.right > SCREEN_WIDTH:
              self.rect.right = SCREEN_WIDTH
        if self.rect.top <= 0:
              self.rect.top = 0
        if self.rect.bottom >= SCREEN_HEIGHT:
              self.rect.bottom = SCREEN_HEIGHT

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

all_sprites = pygame.sprite.Group()
gates = pygame.sprite.Group()
all_sprites.add(player)
all_sprites.add(qubit)
all_sprites.add(H)
all_sprites.add(X)
gates.add(H)
gates.add(X)

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

    player.update(pressed_keys)

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