import numpy as np
import pygame

class Bomb():
    def __init__(self):
        self.max_time = 200
        self.time = 200
        self.quantum_state = np.array([1,0])

    def decrease_timer(self):
        if(self.time > 0): self.time -= 1

    def get_time_ratio(self):
        return(self.time/self.max_time)

    def update_state(self, matrix):
        self.quantum_state = np.dot(matrix,self.quantum_state)
        rho = np.angle(self.quantum_state[0])
        if rho != 0:
            self.quantum_state*=np.exp(-rho*1j)
        self.quantum_state = np.real_if_close(self.quantum_state)

    def measurement(self):
        # probability = np.power(np.sin(self.quantum_state[0]/2),2)
        probability = np.power(np.abs(self.quantum_state[0]),2)
        measurement_result = int(np.random.binomial(1, probability))
        return(measurement_result)
    

class Timer(pygame.sprite.Sprite):
    def __init__(self, topleft, TILE_SIZE):
        super(Timer, self).__init__()
        self.TILE_SIZE = TILE_SIZE
        self.topleft = topleft
        self.width = 2*TILE_SIZE-35
        self.height = 10
        self.surf = pygame.Surface((self.width,self.height))
        self.surf.fill((255,0,0))
        self.rect = self.surf.get_rect(topleft=topleft)
    
    def update(self, ratio):
        self.surf = pygame.Surface((ratio*self.width,self.height))
        self.surf.fill((255,0,0))
        self.rect = self.surf.get_rect(topleft=self.topleft)

class GUI_timer(pygame.sprite.Sprite):
    def __init__(self, topleft, TILE_SIZE):
        super(GUI_timer, self).__init__()
        self.margin = 4
        self.surf = pygame.Surface((2*TILE_SIZE+self.margin,25+self.margin))
        self.surf.fill((255,255,255))
        self.rect = self.surf.get_rect(topleft=(topleft[0]-self.margin/2,topleft[1]-self.margin/2))