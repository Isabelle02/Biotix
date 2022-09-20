#if UNITY_EDITOR

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class LevelsEditor : EditorWindow
{
    private ReorderableList _levelsList;
    private ReorderableList _playersList;
    private ReorderableList _aisList;
    private ReorderableList _teamNodesList;

    private Vector2 _pos;

    private LevelsConfig _levelsConfig;

    private bool _isPlayerTeam;
    
    [MenuItem("Window/LevelsEditorWindow")]
    private static void Init() => GetWindow<LevelsEditor>("LevelsEditorWindow", true);

    private void OnEnable()
    {
        _levelsConfig = Resources.Load<LevelsConfig>("Configs/LevelsConfig");
        
        _levelsList = new ReorderableList(_levelsConfig.LevelsData, typeof(LevelData), 
            true, true, true, true)
        {
            drawElementCallback = (rect, index, _, _) =>
            {
                rect.y += 2;
                EditorGUI.LabelField(rect, $"Level {index}");
            },
            drawHeaderCallback = rect => 
            {  
                EditorGUI.LabelField(rect, "Levels");
            },
            onAddCallback = list =>
            {
                _levelsConfig.LevelsData.Add(new LevelData());
            },
            onRemoveCallback = list =>
            {
                _levelsConfig.LevelsData.RemoveAt(list.index);
            },
            onSelectCallback = list =>
            {
                _playersList = new ReorderableList(_levelsConfig.LevelsData[list.index].PlayersData, typeof(PlayerData), 
                    true, true, true, true)
                {
                    drawElementCallback = (rect, index, _, _) =>
                    {
                        rect.y += 2;
                        var teamData = _levelsConfig.LevelsData[list.index].PlayersData[index];
                        teamData.TeamID = EditorGUI.IntField(rect, $"{teamData.GetType()} Team", teamData.TeamID);
                    },
                    drawHeaderCallback = rect => 
                    {  
                        EditorGUI.LabelField(rect, "Player Teams");
                    },
                    onAddCallback = tList =>
                    {
                        _levelsConfig.LevelsData[list.index].PlayersData.Add(new PlayerData());
                    },
                    onRemoveCallback = tList =>
                    {
                        _levelsConfig.LevelsData[list.index].PlayersData.RemoveAt(tList.index);
                    },
                    onSelectCallback = tList =>
                    {
                        _isPlayerTeam = true;
                        
                        _teamNodesList = new ReorderableList(_levelsConfig.LevelsData[list.index].PlayersData[tList.index].NodesData, typeof(NodeData), 
                            true, true, true, true)
                        {
                            drawElementCallback = (rect, index, _, _) =>
                            {
                                rect.y += 2;
                                rect.xMax /= 3;

                                var node = _levelsConfig.LevelsData[list.index].PlayersData[tList.index]
                                    .NodesData[index];
                                node.TeamID = _levelsConfig.LevelsData[list.index].PlayersData[tList.index].TeamID;
                                node.Position = EditorGUI.Vector3Field(rect, string.Empty, node.Position);

                                rect.x = rect.xMax;
                                node.Radius = EditorGUI.FloatField(rect, "Radius", node.Radius);

                                rect.x = rect.xMax;
                                node.MaxUnitCount = EditorGUI.IntField(rect, "Max units", node.MaxUnitCount);
                            },
                            drawHeaderCallback = rect => 
                            {  
                                EditorGUI.LabelField(rect, "Nodes");
                            }
                        };
                        
                        LoadLevel(list.index);
                    }
                };
                
                 _aisList = new ReorderableList(_levelsConfig.LevelsData[list.index].AisData, typeof(AIData), 
                    true, true, true, true)
                {
                    drawElementCallback = (rect, index, _, _) =>
                    {
                        rect.y += 2;
                        var teamData = _levelsConfig.LevelsData[list.index].AisData[index];
                        teamData.TeamID = EditorGUI.IntField(rect, $"{teamData.GetType()} Team", teamData.TeamID);
                    },
                    drawHeaderCallback = rect => 
                    {  
                        EditorGUI.LabelField(rect, "AI Teams");
                    },
                    onAddCallback = tList =>
                    {
                        _levelsConfig.LevelsData[list.index].AisData.Add(new AIData());
                    },
                    onRemoveCallback = tList =>
                    {
                        _levelsConfig.LevelsData[list.index].AisData.RemoveAt(tList.index);
                    },
                    onSelectCallback = tList =>
                    {
                        _isPlayerTeam = false;
                        
                        _teamNodesList = new ReorderableList(_levelsConfig.LevelsData[list.index].AisData[tList.index].NodesData, typeof(NodeData), 
                            true, true, true, true)
                        {
                            drawElementCallback = (rect, index, _, _) =>
                            {
                                rect.y += 2;
                                rect.xMax /= 3;

                                var node = _levelsConfig.LevelsData[list.index].AisData[tList.index].NodesData[index];
                                node.TeamID = _levelsConfig.LevelsData[list.index].AisData[tList.index].TeamID;
                                node.Position = EditorGUI.Vector3Field(rect, string.Empty, node.Position);

                                rect.x = rect.xMax;
                                node.Radius = EditorGUI.FloatField(rect, "Radius", node.Radius);

                                rect.x = rect.xMax;
                                node.MaxUnitCount = EditorGUI.IntField(rect, "Max units", node.MaxUnitCount);
                            },
                            drawHeaderCallback = rect => 
                            {  
                                EditorGUI.LabelField(rect, "Nodes");
                            }
                        };
                        
                        LoadLevel(list.index);
                    }
                };
                
                LoadLevel(list.index);
            }
        };
        
        SceneView.duringSceneGui += OnSceneGUI;
    }
    
    private void OnSceneGUI(SceneView scene)
    {
        if (Event.current.type == EventType.MouseUp)
        {
            var mousePosition = (Vector3) Event.current.mousePosition;
            var ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            mousePosition = new Vector3(ray.origin.x, ray.origin.y, 0);
            var newPos = mousePosition;
            
            if (_levelsList.index >= 0 && _teamNodesList.index >= 0)
            {
                if (_playersList.index >= 0 && _isPlayerTeam)
                    _levelsConfig.LevelsData[_levelsList.index].PlayersData[_playersList.index]
                        .NodesData[_teamNodesList.index].Position = newPos;
                
                if (_aisList.index >= 0 && !_isPlayerTeam)
                    _levelsConfig.LevelsData[_levelsList.index].AisData[_aisList.index]
                        .NodesData[_teamNodesList.index].Position = newPos;
                
                LoadLevel(_levelsList.index);
            }
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        _pos = EditorGUILayout.BeginScrollView(_pos);

        GUILayout.Label("Level Settings", EditorStyles.boldLabel);

        if (_levelsList != null)
        {
            _levelsList.DoLayoutList();
            if (GUILayout.Button("Load"))
            {
                LoadLevel(_levelsList.index);
            }

            if (GUILayout.Button("Save"))
            {
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }
        
        _playersList?.DoLayoutList();
        _aisList?.DoLayoutList();
        
        _teamNodesList?.DoLayoutList();
        
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void LoadLevel(int index)
    {
        LevelManager.ResetLevel();
        var canvas = FindObjectOfType<Canvas>();
        var lvl = _levelsConfig.LevelsData[index];
        
        foreach (var t in lvl.PlayersData)
        {
            foreach (var n in t.NodesData)
            {
                var nodeObj = Instantiate(_levelsConfig.NodeView, canvas.transform);
                nodeObj.transform.position = n.Position;
                nodeObj.SetSprite(_levelsConfig.NodeSprites[n.TeamID]);
                nodeObj.SetUnitCountText(n.MaxUnitCount.ToString());
                nodeObj.SetRadius(n.Radius);
            }
        }
        
        foreach (var t in lvl.AisData)
        {
            foreach (var n in t.NodesData)
            {
                var nodeObj = Instantiate(_levelsConfig.NodeView, canvas.transform);
                nodeObj.transform.position = n.Position;
                nodeObj.SetSprite(_levelsConfig.NodeSprites[n.TeamID]);
                nodeObj.SetUnitCountText(n.MaxUnitCount.ToString());
                nodeObj.SetRadius(n.Radius);
            }
        }
    }
}

#endif