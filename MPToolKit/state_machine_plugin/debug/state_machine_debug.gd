extends CanvasLayer

export var pathToStateMachine : NodePath

const BASE_NAME = "State: "
const METHOD_NAME = "StateChanged"
onready var label = $RichTextLabel
var state_machine

func _ready():
	get_node(pathToStateMachine).connect(METHOD_NAME, self, "_on_StateMachine_StateChanged")
	pass


func _on_StateMachine_StateChanged(new_state):
	var node = new_state as Node
	label.text = BASE_NAME + node.name
	pass
