#if UNITY_EDITOR

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class LevelsEditor : EditorWindow
{
    private ReorderableList _levelsList;
    private ReorderableList _teamsList;
    private ReorderableList _teamNodesList;

    private Vector2 _pos;

    private LevelsConfig _levelsConfig;
    
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
                _teamsList = new ReorderableList(_levelsConfig.LevelsData[list.index].TeamsData, typeof(TeamData), 
                    true, true, true, true)
                {
                    drawElementCallback = (rect, index, _, _) =>
                    {
                        rect.y += 2;
                        _levelsConfig.LevelsData[list.index].TeamsData[index].TeamID =
                            EditorGUI.IntField(rect, "Team",
                                _levelsConfig.LevelsData[list.index].TeamsData[index].TeamID);
                    },
                    drawHeaderCallback = rect => 
                    {  
                        EditorGUI.LabelField(rect, "Teams");
                    },
                    onAddCallback = tList =>
                    {
                        _levelsConfig.LevelsData[list.index].TeamsData.Add(new TeamData());
                    },
                    onRemoveCallback = tList =>
                    {
                        _levelsConfig.LevelsData[list.index].TeamsData.RemoveAt(tList.index);
                    },
                    onSelectCallback = tList =>
                    {
                        _teamNodesList = new ReorderableList(_levelsConfig.LevelsData[list.index].TeamsData[tList.index].NodesData, typeof(NodeData), 
                            true, true, true, true)
                        {
                            drawElementCallback = (rect, index, _, _) =>
                            {
                                rect.y += 2;
                                rect.xMax /= 3;

                                var node = _levelsConfig.LevelsData[list.index].TeamsData[tList.index]
                                    .NodesData[index];
                                node.TeamID = _levelsConfig.LevelsData[list.index].TeamsData[tList.index].TeamID;
                                node.Position = EditorGUI.Vector3Field(rect, string.Empty, node.Position);

                                rect.x = rect.xMax;
                                node.Radius = EditorGUI.FloatField(rect, "Rad", node.Radius);

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
            if (_levelsList.index >= 0 && _teamsList.index >= 0 && _teamNodesList.index >= 0)
            {
                var mousePosition = (Vector3) Event.current.mousePosition;
                var ray = HandleUtility.GUIPointToWorldRay(mousePosition);
                mousePosition = new Vector3(ray.origin.x, ray.origin.y, 0);
                var newPos = mousePosition;
                _levelsConfig.LevelsData[_levelsList.index].TeamsData[_teamsList.index]
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

        _teamsList?.DoLayoutList();
        
        _teamNodesList?.DoLayoutList();
        
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void LoadLevel(int index)
    {
        LevelManager.ResetLevel();
        var canvas = FindObjectOfType<Canvas>();
        var lvl = _levelsConfig.LevelsData[index];
        foreach (var t in lvl.TeamsData)
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