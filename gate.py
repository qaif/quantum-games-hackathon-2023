import pygame
import numpy as np


gates_ids = {'M':4, 'H':5, 'X':6, 'RY3':7, 'RY4':8, 'RZ2':9}
gates_ids_inv = {4:'M', 5:'H', 6:'X', 7:'RY3', 8:'RY4', 9:'RZ2'}
gates_matrices = {
    'M':None,
    'H':1/np.sqrt(2)*np.array([[1,1],[1,-1]]),
    'X':np.array([[0,1],[1,0]]),
    'RY3':np.array([[np.cos(np.pi/6),-np.sin(np.pi/6)],[np.sin(np.pi/6),np.cos(np.pi/6)]]),
    'RY4':np.array([[np.cos(np.pi/8),-np.sin(np.pi/8)],[np.sin(np.pi/8),np.cos(np.pi/8)]]),
    'RZ2':np.array([[1,0],[0,np.exp(np.pi/2*1j)]])
} 

class Gate(pygame.sprite.Sprite):
    def __init__(self, gate_type, topleft):
        super(Gate, self).__init__()
        self.gate_type = gate_type
        self.id = gates_ids[gate_type]
        self.matrix = gates_matrices[gate_type]
        self.surf = pygame.image.load('imgs/gates/'+gate_type+'.png')
        self.rect = self.surf.get_rect(topleft=topleft)