extends CharacterBody3D

const SPEED := 5.0
const MOUSE_SENSITIVITY := 0.003

@onready var camera: Camera3D = $Camera3D

func _ready() -> void:
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

	# Position player at the correct spawn point for this scene.
	var spawn_name := "Spawn_" + GameState.spawn_target.capitalize()
	var spawn_node := get_tree().current_scene.find_child(spawn_name, true, false)
	if spawn_node:
		global_position = spawn_node.global_position

func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		rotate_y(-event.relative.x * MOUSE_SENSITIVITY)
		camera.rotate_x(-event.relative.y * MOUSE_SENSITIVITY)
		camera.rotation.x = clamp(camera.rotation.x, -PI / 2.0, PI / 2.0)
	if event.is_action_pressed("ui_cancel"):
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

func _physics_process(delta: float) -> void:
	if not is_on_floor():
		velocity += get_gravity() * delta

	var forward := -transform.basis.z
	var right   :=  transform.basis.x
	var move_dir := Vector3.ZERO

	if Input.is_key_pressed(KEY_W): move_dir += forward
	if Input.is_key_pressed(KEY_S): move_dir -= forward
	if Input.is_key_pressed(KEY_A): move_dir -= right
	if Input.is_key_pressed(KEY_D): move_dir += right

	move_dir.y = 0.0
	if move_dir.length_squared() > 0.0:
		move_dir = move_dir.normalized()
		velocity.x = move_dir.x * SPEED
		velocity.z = move_dir.z * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0.0, SPEED)
		velocity.z = move_toward(velocity.z, 0.0, SPEED)

	move_and_slide()
