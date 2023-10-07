import pygame
from qiskit.visualization import plot_bloch_vector
import matplotlib.pyplot as plt
import io
import numpy as np

class Qubit(pygame.sprite.Sprite):
    def __init__(self, topright):
        super(Qubit, self).__init__()
        self.topright = topright
        self.coords = [0,0]
        self.last_update = 0
        self.surf = None
        self.rect = None
        self.reLoadImage()

    def reLoadImage(self):
        buffer = io.BytesIO()
        plot_bloch_vector([1]+self.coords, coord_type='spherical', figsize=(1.8,1.8))
        plt.savefig(buffer, format="png", transparent=True)
        plt.close()
        buffer.seek(0)
        self.surf = pygame.image.load(buffer).convert().convert_alpha()
        self.rect = self.surf.get_rect(topright=self.topright)

gates_ids = {'H':5, 'X':6, 'RY3':7, 'RY4':8, 'RZ2':9}
gates_matrices = {
    'H':1/np.sqrt(2)*np.array([[1,1],[1,-1]]),
    'X':np.array([[0,1],[1,0]]),
    'RY3':np.array([[np.cos(np.pi/6),-np.sin(np.pi/6)],[np.sin(np.pi/6),np.cos(np.pi/6)]]),
    'RY4':np.array([[np.cos(np.pi/8),-np.sin(np.pi/8)],[np.sin(np.pi/8),np.cos(np.pi/8)]]),
    'RZ2':np.array([[1,0],[0,np.exp(np.pi/2*1j)]])
}
