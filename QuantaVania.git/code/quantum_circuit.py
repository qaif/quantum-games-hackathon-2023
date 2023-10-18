import pygame
from qiskit import QuantumRegister, QuantumCircuit
import numpy as np

from pygame.image import load as loadImage
from settings import *


class QuantumCircuitGridBackground(pygame.sprite.Sprite):
    def __init__(self, qc_grid_model):
        super().__init__()
        self.qc_grid_model = qc_grid_model
        self.width = QUANTUM_CIRCUIT_TILE_SIZE * (self.qc_grid_model.num_columns + 2)
        self.height = QUANTUM_CIRCUIT_TILE_SIZE * (self.qc_grid_model.num_qubits + 1)
        
        # BACKGROUND SURFACE
        self.image = pygame.Surface((self.width, self.height))
        self.image.fill(QUANTUM_CIRCUIT_BG_COLOR)
        self.rect = self.image.get_rect()
        self.rect.inflate_ip(-WIRE_LINE_WIDTH, -WIRE_LINE_WIDTH)

        self.run()

    def draw_qubit_wires(self):
        for wire in range(self.qc_grid_model.num_qubits):
            x_start = QUANTUM_CIRCUIT_TILE_SIZE * 0.5
            x_end = self.width - (QUANTUM_CIRCUIT_TILE_SIZE * 0.5)
            y = (wire + 1) * QUANTUM_CIRCUIT_TILE_SIZE 
            pygame.draw.line(
                self.image, 
                QUANTUM_CIRCUIT_WIRE_COLOR, 
                (x_start, y), 
                (x_end, y),
                WIRE_LINE_WIDTH
            )

    def run(self):
        # Drawing
        pygame.draw.rect(self.image, QUANTUM_CIRCUIT_WIRE_COLOR, self.rect, WIRE_LINE_WIDTH)
        self.draw_qubit_wires()

class QuantumCircuitGridMarker(pygame.sprite.Sprite):
    def __init__(self):
        super().__init__()
        self.image = loadImage("graphics/quantum_circuit_gates/circuit-grid-cursor.png").convert_alpha()
        self.rect = self.image.get_rect()

class QuantumCircuitGridNode:
    def __init__(self, gate_type, rotation_angle = 0.0, first_ctrl = -1, second_ctrl = -1, swap = -1):
        self.gate_type = gate_type # What Gate is at this node
        self.rotation_angle = rotation_angle # If radian != 0 then this node have a U(theta) gate; Ex:- RX, RY, RZ
        self.first_ctrl = first_ctrl # If first_ctrl > 0; then this node is a controlled gate with one control node # It's value will be the wire number on which the first control is placed
        self.second_ctrl = second_ctrl # If second_ctrl > 0; then this node is a controlled gate with two control nodes # It's value will be the wire number on which the second control is placed
        self.swap = swap # If swap != -1 then this node have a swap gate

    def __str__(self):
        string = "Type: " + str(self.gate_type)
        string += ", rotation_angle: " + str(self.rotation_angle) if self.rotation_angle != 0 else ""
        string += ", ctrl_a: " + str(self.first_ctrl) if self.first_ctrl != -1 else ""
        string += ", ctrl_b: " + str(self.second_ctrl) if self.second_ctrl != -1 else ""
        return string

class QuantumCircuitGridGate(pygame.sprite.Sprite):
    def __init__(self, qc_grid_model, wire, column):
        super().__init__()
        self.qc_grid_model = qc_grid_model
        
        # Gate Position
        self.wire = wire
        self.column = column

        self.run()
    
    def import_gate(self, gate_name, colorkey = None):
        gate_image_folder = "graphics/quantum_circuit_gates"
        gate_image = loadImage(f"{gate_image_folder}/{gate_name}")
        if colorkey is not None:
            if colorkey == -1:
                colorkey = gate_image.get_at((0,0))
            gate_image.set_colorkey(colorkey)
        return gate_image, gate_image.get_rect()
    
    def load_gate(self):
        gate = self.qc_grid_model.get_gate_at_node(self.wire, self.column)
        
        if gate == GATES['IDENTITY']:
            self.image, self.rect = self.import_gate("iden_gate.png", -1)    
        
        elif gate == GATES['X']:
            node = self.qc_grid_model.get_node(self.wire, self.column)
            # Check if this is a CNOT Gate
            if node.first_ctrl >= 0 or node.second_ctrl >= 0:
                if self.wire > max(node.ctrl_a, node.ctrl_b): # If target wire is below control wire
                    self.image, self.rect = self.import_gate("not_gate_below_ctrl.png", -1)
                else: # If target wire is above control wire
                    self.image, self.rect = self.import_gate("not_gate_above_ctrl.png", -1)
            elif node.rotation_angle != 0: # Else If this is a RX Gate
                self.image, self.rect = self.import_gate("rx_gate.png", -1)
                # Draw the value of theta as an arc of a circle 
                pygame.draw.arc(self.image, QUANTUM_GATE_PHASE_COLOR, self.rect, 0, node.rotation_angle % (2 * np.pi), 4)
            else: # Else if this is a normal X Gate
                self.image, self.rect = self.import_gate("x_gate.png", -1)
        
        elif gate == GATES['Y']:
            node = self.qc_grid_model.get_node(self.wire, self.column)
            # Check if this is a RY Gate
            if node.rotation_angle != 0:
                self.image, self.rect = self.import_gate("ry_gate.png", -1)
                # Draw the value of theta as an arc of a circle 
                pygame.draw.arc(self.image, QUANTUM_GATE_PHASE_COLOR, self.rect, 0, node.rotation_angle % (2 * np.pi), 4)
            else: # Else if this is a normal Y Gate
                self.image, self.rect = self.import_gate("y_gate.png", -1)
        
        elif gate == GATES['Z']:
            node = self.qc_grid_model.get_node(self.wire, self.column)
            # Check if this is a RY Gate
            if node.rotation_angle != 0:
                self.image, self.rect = self.import_gate("rz_gate.png", -1)
                # Draw the value of theta as an arc of a circle 
                pygame.draw.arc(self.image, QUANTUM_GATE_PHASE_COLOR, self.rect, 0, node.rotation_angle % (2 * np.pi), 4)
            else: # Else if this is a normal Y Gate
                self.image, self.rect = self.import_gate("z_gate.png", -1)
        
        elif gate == GATES['S']:
            self.image, self.rect = self.import_gate("s_gate.png", -1)
        
        elif gate == GATES['SDG']:
            self.image, self.rect = self.import_gate("sdg_gate.png", -1)
        
        elif gate == GATES['T']:
            self.image, self.rect = self.import_gate("t_gate.png", -1)
        
        elif gate == GATES['TDG']:
            self.image, self.rect = self.import_gate("tdg_gate.png", -1)
        
        elif gate == GATES['H']:
            self.image, self.rect = self.import_gate("h_gate.png", -1)
        
        elif gate == GATES['SWAP']:
            self.image, self.rect = self.import_gate("swap_gate.png", -1)
        
        elif gate == GATES['CTRL']:
            # Check if the target wire is above the control wire
            if self.wire > self.qc_grid_model.get_wire_for_control_node_at(self.wire, self.column):
                self.image, self.rect = self.import_gate("ctrl_gate_bottom_wire.png", -1)
            else: # if the target wire is above the control wire
                self.image, self.rect = self.import_gate("ctrl_gate_top_wire.png", -1)
        
        elif gate == GATES['CTRL_LINE']:
            self.image, self.rect = self.import_gate("ctrl_line_gate.png", -1)
        
        else: # If the node is empty
            # Draw a transparent block, i.e., empty gate/node
            self.image = pygame.Surface([GATE_TILE_WIDTH, GATE_TILE_HIEGHT])
            self.image.set_alpha(0)
            self.rect = self.image.get_rect()
        
        self.image.convert()

    def run(self):
        self.load_gate()

class QuantumCircuitGridModel():
    def __init__(self, num_qubits, num_columns):
        self.num_qubits = num_qubits
        self.num_columns = num_columns
        self.nodes = np.zeros(
            (self.num_qubits, self.num_columns),
            dtype=QuantumCircuitGridNode
        )
    
    def __str__(self):
        string = "CircuitGridModel:\n"
        for wire in range(self.num_qubits):
            row_values = [str(self.get_gate_at_node(wire, column)) for column in range(self.num_columns)]
            string += ", ".join(row_values) + "\n"
        return string

    def set_node(self, wire, column, qc_grid_node):
        self.nodes[wire][column] = QuantumCircuitGridNode(
            qc_grid_node.gate_type,
            qc_grid_node.rotation_angle,
            qc_grid_node.first_ctrl,
            qc_grid_node.second_ctrl,
            qc_grid_node.swap
        )
    
    def get_node(self, wire, column):
        return self.nodes[wire][column]

    def get_gate_at_node(self, wire, column):
        node = self.nodes[wire][column]
        
        if node and node.gate_type != GATES['EMPTY']: # If the node is already occupied
            return node.gate_type # Return the gate occupying the node
        
        column_nodes = self.nodes[:, column]
        for index, other_node in enumerate(column_nodes):
            if index != wire and other_node:
                # Check if the other_node is a control node
                if other_node.first_ctrl == wire or other_node.second_ctrl == wire:
                    return GATES['CTRL']
                # Or if it is a swap node
                elif other_node.swap == wire:
                    return GATES['SWAP']
        
        # If no gate is present at the node return 'EMPTY'
        return GATES['EMPTY']

    def get_wire_for_control_node_at(self, control_wire, column):
        control_wire = -1
        column_nodes = self.nodes[:, column]

        for index in range(self.num_qubits):
            if index != control_wire:
                other_node = column_nodes[index]
                if other_node:
                    if (other_node.first_ctrl == control_wire or other_node.second_ctrl == control_wire):
                        control_wire = index
                        print("Found ", self.get_gate_at_node(control_wire, column), " on wire ", control_wire)
        
        return control_wire

    def create_quantum_circuit(self):
        """Create Quantum Circuit from Quantum Circuit Grid"""
        qr = QuantumRegister(self.num_qubits, "q")
        qc = QuantumCircuit(qr)

        for column in range(self.num_columns):
            for wire in range(self.num_qubits):
                node = self.nodes[wire][column]
                
                if node:
                    if node.gate_type == GATES['IDENTITY']:
                        qc.i(qr[wire])

                    elif node.gate_type == GATES['X']:
                        if node.rotation_angle == 0: # Node have a Pauli X Gate or Controlled X Gate or Toffoli Gate
                            if node.first_ctrl != -1: # If first control is active
                                if node.second_ctrl != -1: # If second control is also active then node have a Toffoli Gate
                                    qc.ccx(qr[node.frist_ctrl], qr[node.second_ctrl], qr[wire])
                                else: # Node have a Controlled X Gate
                                    qc.cx(qr[node.frist_ctrl], qr[wire])
                            else: # If no control is active then the node have a Pauli X Gate
                                qc.x(qr[wire])
                        else: # If angle is not zero then it is a RX Gate
                            qc.rx(node.rotation_angle, qr[wire])
                    
                    elif node.gate_type == GATES['Y']:
                        if node.rotation_angle == 0: # Node have a Pauli Y Gate or Controlled Y Gate
                            if node.first_ctrl != -1: # If first control is active then Node have a CY Gate
                                qc.cy(qr[self.first_ctrl], qr[wire])
                            else: # If no control is active then the node have a Pauli Y Gate
                                qc.y(qr[wire])
                    
                    elif node.gate_type == GATES['Z']:
                        if node.rotation_angle == 0: # Node have a Pauli Z Gate or Controlled Z Gate
                            if node.first_ctrl != -1: # If first control is active then Node have a CZ Gate
                                qc.cz(qr[self.first_ctrl], qr[wire])
                            else: # If no control is active then the node have a Pauli Z Gate
                                qc.z(qr[wire])
                    
                    elif node.gate_type == GATES['S']:
                        qc.s(qr[wire])
                    
                    elif node.gate_type == GATES['SDG']:
                        qc.sdg(qr[wire])
                    
                    elif node.gate_type == GATES['T']:
                        qc.t(qr[wire])
                    
                    elif node.gate_type == GATES['TDG']:
                        qc.tdg(qr[wire])
                    
                    elif node.gate_type == GATES['H']:
                        if node.first_ctrl != -1: # If first control is active then Node have a CH Gate
                            qc.ch(qr[self.first_ctrl], qr[wire])
                        else: # If no control is active then the node have a H Gate
                            qc.h(qr[wire])
                    
                    elif node.gate_type == GATES['SWAP']:
                        if node.first_ctrl != -1: # If first control is active then Node have a Controlled Swap Gate
                            qc.cswap(qr[self.first_ctrl], qr[wire])
                        else: # If no control is active then the node have a Swap Gate
                            qc.swap(qr[wire])
        
        return qc

class QuantumCircuitGrid(pygame.sprite.RenderPlain):
    def __init__(self, position, num_qubits, num_columns):
        super().__init__()
        
        ## State
        self.position = position
        self.current_wire = 0
        self.current_column = 0
        
        self.qc_grid_model = QuantumCircuitGridModel(num_qubits, num_columns)
        self.qc_grid_background = QuantumCircuitGridBackground(self.qc_grid_model)
        self.qc_grid_marker = QuantumCircuitGridMarker()

        self.gate_tiles = np.zeros(
            (self.qc_grid_model.num_qubits, self.qc_grid_model.num_columns),
            dtype=QuantumCircuitGridGate
        )
    
    ## SUPPORT FUNCTIONS
    def highlight_current_node(self, wire, column):
        self.current_wire = wire
        self.current_column = column
        self.qc_grid_marker.rect.topleft = (
            self.position[0] + QUANTUM_CIRCUIT_TILE_SIZE * (self.current_column + 1.2),
            self.position[1] + QUANTUM_CIRCUIT_TILE_SIZE * (self.current_wire + 0.7)
        )
    
    def get_gate_at_current_node(self):
        return self.qc_grid_model.get_gate_at_node(self.current_wire, self.current_column)
    
    ## HANDLE UPDATES    
    def update_sprites(self):
        for sprite in self.sprites():
            sprite.update()
    
    def update_qc_grid_background(self):
        self.qc_grid_background.rect.topleft = self.position
    
    def updage_gate_tiles(self):
        for wire in range(self.qc_grid_model.num_qubits):
            for column in range(self.qc_grid_model.num_columns):
                gate_tile = self.gate_tiles[wire][column]
                gate_tile.rect.center = (
                    self.position[0] + QUANTUM_CIRCUIT_TILE_SIZE * (column + 1.5),
                    self.position[1] + QUANTUM_CIRCUIT_TILE_SIZE * (wire + 1)
                )
    
    def update(self):
        self.update_sprites()
        self.update_qc_grid_background()
        self.updage_gate_tiles()
        self.highlight_current_node(self.current_wire, self.current_column)
    
    ## HANDLE INPUTS
    def move_to_adjacent_node(self, direction):
        if(direction == QUANTUM_CIRCUIT_MARKER_MOVE_LEFT and self.current_column > 0):
            self.current_column -= 1
        elif (direction == QUANTUM_CIRCUIT_MARKER_MOVE_RIGHT and self.current_column < self.qc_grid_model.num_columns - 1):
            self.current_column += 1
        elif (direction == QUANTUM_CIRCUIT_MARKER_MOVE_UP and self.current_wire > 0):
            self.current_wire -= 1
        elif (direction == QUANTUM_CIRCUIT_MARKER_MOVE_DOWN and self.current_wire < self.qc_grid_model.num_qubits - 1):
            self.current_wire += 1

        self.highlight_current_node(self.current_wire, self.current_column)

    def handle_input_x(self):
        gate_at_current_node = self.get_gate_at_current_node()
        if gate_at_current_node == GATES['EMPTY']:
            qc_grid_node = QuantumCircuitGridNode(GATES['X'])
            self.qc_grid_model.set_node(self.current_wire, self.current_column, qc_grid_node)
        self.update()
    
    def handle_input_y(self):
        gate_at_current_node = self.get_gate_at_current_node()
        if gate_at_current_node == GATES['EMPTY']:
            qc_grid_node = QuantumCircuitGridNode(GATES['Y'])
            self.qc_grid_model.set_node(self.current_wire, self.current_column, qc_grid_node)
        self.update()
    
    def handle_input_z(self):
        gate_at_current_node = self.get_gate_at_current_node()
        if gate_at_current_node == GATES['EMPTY']:
            qc_grid_node = QuantumCircuitGridNode(GATES['Z'])
            self.qc_grid_model.set_node(self.current_wire, self.current_column, qc_grid_node)
        self.update()
    
    def handle_input_h(self):
        gate_at_current_node = self.get_gate_at_current_node()
        if gate_at_current_node == GATES['EMPTY']:
            qc_grid_node = QuantumCircuitGridNode(GATES['H'])
            self.qc_grid_model.set_node(self.current_wire, self.current_column, qc_grid_node)
        self.update()
    
    def handle_input_delete(self, wire, column):
        gate_at_current_node = self.qc_grid_model.get_gate_at_node(wire, column)
        if(
            gate_at_current_node == GATES['X']
            or gate_at_current_node == GATES['Y']
            or gate_at_current_node == GATES['Z']
            or gate_at_current_node == GATES['H']
        ):
            self.delete_controls_for_gate(wire, column)

        if gate_at_current_node == GATES['CTRL']:
            gate_wire = self.qc_grid_model.get_wire_for_control_node_at(wire, column)
            if gate_wire >= 0:
                self.delete_controls_for_gate(gate_wire, column)
        elif (
            gate_at_current_node != GATES['CTRL']
            and gate_at_current_node != GATES['SWAP']
            and gate_at_current_node != GATES['CTRL_LINE']
        ):
            qc_grid_node = QuantumCircuitGridNode(GATES['EMPTY'])
            self.qc_grid_model.set_node(wire, column, qc_grid_node)
        
        self.update()

    def handle_input_clear_all(self):
        for wire in range(self.qc_grid_model.num_qubits):
            for column in range(self.qc_grid_model.num_columns):
                self.handle_input_delete(wire, column)

    def handle_input_ctrl(self):
        gate_at_current_node = self.get_gate_at_current_node()
        if(
            gate_at_current_node == GATES['X']
            or gate_at_current_node == GATES['Y']
            or gate_at_current_node == GATES['Z']
            or gate_at_current_node == GATES['H']
        ):
            qc_grid_node = self.qc_grid_model.get_node(self.current_wire, self.current_column)
            if qc_grid_node.first_ctrl >= 0:
                # Gate have a control qubit so remove it
                orignal_first_ctrl = qc_grid_node.first_ctrl
                qc_grid_node.first_ctrl = -1
                self.qc_grid_model.set_node(self.current_wire, self.current_column, qc_grid_node)

                # Remove Control Line Nodes
                for wire in range(
                    min(self.current_wire, orignal_first_ctrl) + 1,
                    max(self.current_column, orignal_first_ctrl)
                ):
                    if(self.qc_grid_model.get_gate_at_node(wire, self.current_column) == GATES['CTRL_LINE']):
                        self.qc_grid_model.set_node(wire, self.current_column, QuantumCircuitGridNode(GATES['EMPTY']))
                self.update()
        else:
            # Attempt to place a control qubit beginning with the wire above
            if self.current_wire >= 0:
                if (self.place_ctrl_qubit(self.current_wire, self.current_wire - 1) == -1):
                    if self.current_wire < self.qc_grid_model.num_qubits:
                        if(self.place_ctrl_qubit(self.current_wire, self.current_wire + 1) == -1):
                            print("Can't place control qubit!")
                            self.display_exceptional_condition()

    def handle_input_move_ctrl(self, direction):
        gate_at_current_node = self.get_gate_at_current_node()
        if(
            gate_at_current_node == GATES['X']
            or gate_at_current_node == GATES['Y']
            or gate_at_current_node == GATES['Z']
            or gate_at_current_node == GATES['H']
        ):
            qc_grid_node = self.qc_grid_model.get_node(self.current_wire, self.current_column)
            if 0 <= qc_grid_node.first_ctrl < self.qc_grid_model.num_qubits:
                # Gate already has a control qubit so try to move it
                if direction == QUANTUM_CIRCUIT_MARKER_MOVE_UP:
                    candidate_ctrl_wire = qc_grid_node.first_ctrl - 1
                    if candidate_ctrl_wire == self.current_wire:
                        candidate_ctrl_wire -= 1 # move up to previous wire above
                else:
                    candidate_ctrl_wire = qc_grid_node.first_ctrl + 1
                    if candidate_ctrl_wire == self.current_wire:
                        candidate_ctrl_wire += 1 # Move down to next wire below
            
                if 0 <= candidate_ctrl_wire < self.qc_grid_model.num_qubits:
                    if (self.place_ctrl_quabit(self.current_wire, candidate_ctrl_wire) == candidate_ctrl_wire):
                        if (direction == QUANTUM_CIRCUIT_MARKER_MOVE_UP and candidate_ctrl_wire < self.current_wire):
                            if (self.qc_grid_model.get_gate_at_node(candidate_ctrl_wire + 1, self.current_column) == GATES['EMPTY']):
                                self.qc_grid_model.set_node(candidate_ctrl_wire + 1, self.current_column, QuantumCircuitGridNode(GATES['CTRL_LINE']))
                        elif(direction == QUANTUM_CIRCUIT_MARKER_MOVE_DOWN and candidate_ctrl_wire > self.current_wire):
                            if (self.qc_grid_model.get_gate_at_node(candidate_ctrl_wire - 1, self.current_column) == GATES['EMPTY']):
                                self.qc_grid_model.set_node(candidate_ctrl_wire - 1, self.current_column, QuantumCircuitGridNode(GATES['CTRL_LINE']))
                        
                        print("Control qubit placed on the wire ", candidate_ctrl_wire, " successfully!")
                        self.update()
                    
                    else:
                        print("Control qubit could not be placed on the wire ", candidate_ctrl_wire, " successfully!")

    def handle_input_rotate(self, rotation_angle):
        gate_at_current_node = self.get_gate_at_current_node()
        if(
            gate_at_current_node == GATES['X']
            or gate_at_current_node == GATES['Y']
            or gate_at_current_node == GATES['Z']
        ):
            qc_grid_node = self.qc_grid_model.get_node(self.current_wire, self.current_column)
            qc_grid_node.rotation_angle = (qc_grid_node.rotation_angle + rotation_angle) % 2 * np.pi
            self.qc_grid_model.set_node(self.current_wire, self.current_column, qc_grid_node)
        self.update()

    def place_ctrl_qubit(self, gate_wire, candidate_ctrl_wire):
        # Attempt to place a control qubit on a wire. If successful, return the wire number. If not, return -1
        if (candidate_ctrl_wire < 0 or candidate_ctrl_wire >= self.qc_grid_model.num_qubits):
            return -1
        
        candidate_ctrl_wire_gate = self.qc_grid_model.get_gate_at_node(candidate_ctrl_wire, self.current_column)

        if (candidate_ctrl_wire_gate == GATES['EMPTY'] or candidate_ctrl_wire_gate == GATES['CTRL_LINE']):
            qc_grid_node = self.qc_grid_model.get_node(gate_wire, self.current_column)
            self.qc_grid_model.set_node(gate_wire, self.current_column, qc_grid_node)
            self.qc_grid_model.set_node(candidate_ctrl_wire, self.current_column, QuantumCircuitGridNode(GATES['EMPTY']))
            self.update()
            return candidate_ctrl_wire

    def delete_controls_for_gate(self, gate_wire, column):
        first_control_wire = self.qc_grid_model.get_node(gate_wire, column).first_ctrl
        second_control_wire = self.qc_grid_model.get_node(gate_wire, column).second_ctrl

        # Choose the control wire (if any exist) furthest away from the gate wire
        first_control_wire_distance = 0
        second_control_wire_distance = 0

        if first_control_wire >= 0:
            first_control_wire_distance = abs(first_control_wire - gate_wire)

        if second_control_wire >= 0:
            second_control_wire_distance = abs(second_control_wire - gate_wire)

        ctrl_wire = -1
        if first_control_wire_distance > second_control_wire_distance:
            ctrl_wire = first_control_wire
        elif first_control_wire_distance < second_control_wire_distance:
            ctrl_wire = second_control_wire
        
        if ctrl_wire >= 0:
            for wire in range(
                min(gate_wire, ctrl_wire),
                max(gate_wire, ctrl_wire) + 1
            ):
                print("Replacing wire ", wire, " in column ", column)
                qc_grid_node = QuantumCircuitGridNode(GATES['EMPTY'])
                self.qc_grid_model.set_node(wire, column, qc_grid_node)

    def handle_input(self, key):
        match (key):
            case pygame.K_a:
                self.move_to_adjacent_node(QUANTUM_CIRCUIT_MARKER_MOVE_LEFT),
            case pygame.K_d:
                self.move_to_adjacent_node(QUANTUM_CIRCUIT_MARKER_MOVE_RIGHT),
            case pygame.K_w:
                self.move_to_adjacent_node(QUANTUM_CIRCUIT_MARKER_MOVE_UP),
            case pygame.K_s:
                self.move_to_adjacent_node(QUANTUM_CIRCUIT_MARKER_MOVE_DOWN),
            case pygame.K_x:
                self.handle_input_x(),
            case pygame.K_y:
                self.handle_input_y(),
            case pygame.K_z:
                self.handle_input_z(),
            case pygame.K_h:
                self.handle_input_h(),
            case pygame.K_BACKSPACE:
                self.handle_input_delete(self.current_wire, self.current_column),
            case pygame.K_DELETE:
                self.handle_input_clear_all()
            case pygame.K_c:
                self.handle_input_ctrl(),
            case pygame.K_r:
                self.handle_input_move_ctrl(QUANTUM_CIRCUIT_MARKER_MOVE_UP),
            case pygame.K_f:
                self.handle_input_move_ctrl(QUANTUM_CIRCUIT_MARKER_MOVE_DOWN),
            case pygame.K_q:
                self.handle_input_rotate(-np.pi / 8),
            case pygame.K_e:
                self.handle_input_rotate(np.pi / 8)

    ## RUN, DRAW AND UPDATE EVERYTHING
    def run(self):
        ## Create QuantumCircuitGridGate Object for each gate in the qc_circuit_grid
        for wire in range(self.qc_grid_model.num_qubits):
            for column in range(self.qc_grid_model.num_columns):
                self.gate_tiles[wire][column] = QuantumCircuitGridGate(self.qc_grid_model, wire, column)
                self.gate_tiles[wire][column].run()
        
        ## Drawing
        pygame.sprite.RenderPlain.__init__(self, self.qc_grid_background, self.gate_tiles, self.qc_grid_marker)
        
        ## Update
        self.update()
