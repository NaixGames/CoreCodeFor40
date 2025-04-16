class_name  scene_tests
extends GdUnitTestSuite
@warning_ignore('unused_parameter')
@warning_ignore('return_value_discarded')


# Called when the node enters the scene tree for the first time.
func test_audio_test_scene(timeout = 20000) -> void:
    print("running test with timeout " + str(timeout))
    # Create the scene runner for scene `test_scene.tscn`
    var runner := scene_runner("res://CoreTools/AudioManager/Scenes/AudioTest/AudioTest.tscn")

    await runner.simulate_frames(100)

func test_FSM_test_scene(timeout = 20000) -> void:
    print("running test with timeout " + str(timeout))
    # Create the scene runner for scene `test_scene.tscn`
    var runner := scene_runner("res://CoreTools/FSM/Scenes/FSMTest/FSMTest.tscn")

    await runner.simulate_frames(100)

func test_input_remapper_test_scene(timeout = 20000) -> void:
    print("running test with timeout " + str(timeout))
    # Create the scene runner for scene `test_scene.tscn`
    var runner := scene_runner("res://CoreTools/Input/InputRemapper/Scene/InputRemapperTest.tscn")

    await runner.simulate_frames(100)


func test_log_test_scene(timeout = 20000) -> void:
    print("running test with timeout " + str(timeout))
    # Create the scene runner for scene `test_scene.tscn`
    var runner := scene_runner("res://CoreTools/LogManager/Scenes/LogTest/LogTest.tscn")

    await runner.simulate_frames(100)

func test_particle_system_test_scene(timeout = 20000) -> void:
    print("running test with timeout " + str(timeout))
    # Create the scene runner for scene `test_scene.tscn`
    var runner := scene_runner("res://CoreTools/ParticleSystemController/Scenes/ParticleSystemControllerTest.tscn")

    await runner.simulate_frames(100)


func test_scene_transition_test_scene(timeout = 20000) -> void:
    print("running test with timeout " + str(timeout))
    # Create the scene runner for scene `test_scene.tscn`
    var runner := scene_runner("res://CoreTools/SceneTransitionManager/Scene/SceneOne/ExampleScene1.tscn")

    await runner.simulate_frames(100)
