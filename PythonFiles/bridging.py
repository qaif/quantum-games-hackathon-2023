import sys
from qiskit import QuantumCircuit, transpile
from qiskit_aer import AerSimulator
from qiskit.visualization import plot_histogram

def main():
    args = sys.argv
    compute_state(args[0],args[1],args[2])

def compute_state(gate_type, gate_id, size):
    pass