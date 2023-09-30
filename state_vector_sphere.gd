extends Node2D

var DIR = OS.get_executable_path().get_base_dir()
var interpreter_path = DIR.plus_file("PythonFiles/venv/Scripts/activate")
var script_path = DIR.plus_file("PythonFiles/bridging.py")

# Called when the node enters the scene tree for the first time.
func _ready():
	if !OS.has_feature("standalone"):
		interpreter_path = ProjectSettings.globalize_path("res://PythonFiles/venv/Scripts/activate")
		script_path = ProjectSettings.globalize_path("PythonFiles/venv/Scripts/activate")
	var gate_type = ["h","c","x","c","h"]
	var gate_id = [0,0,0,1,1]
	var circuit_size = 2
	compute_state_vector(gate_type, gate_id, circuit_size)

func compute_state_vector(gate_type,gate_id,size):
	OS.execute(interpreter_path, [script_path, gate_type, gate_id, size])
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
