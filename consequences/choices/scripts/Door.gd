extends Area3D

## Scene file to load when the player interacts.
@export var target_scene: String = ""
## Value written to GameState.spawn_target before switching.
@export var spawn_target: String = ""

var _player_nearby := false
var _transitioning := false

func _ready() -> void:
	body_entered.connect(_on_body_entered)
	body_exited.connect(_on_body_exited)

func _unhandled_input(event: InputEvent) -> void:
	if _player_nearby and not _transitioning and event.is_action_pressed("interact"):
		_transitioning = true
		get_viewport().set_input_as_handled()
		GameState.spawn_target = spawn_target
		get_tree().change_scene_to_file(target_scene)

func _on_body_entered(body: Node3D) -> void:
	if body.is_in_group("player"):
		_player_nearby = true
		$HintLabel.visible = true

func _on_body_exited(body: Node3D) -> void:
	if body.is_in_group("player"):
		_player_nearby = false
		$HintLabel.visible = false
