#if UNITY_EDITOR

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class LevelsEditor : EditorWindow
{
    private ReorderableList _levelsList;
    private ReorderableList _nodesList;

    private Vector2 _pos;

    private LevelsConfig _levelsConfig;

    private bool _isWindowActive;
    
    [MenuItem("Window/LevelsEditorWindow")]
    private static void Init() => GetWindow<LevelsEditor>("LevelsEditorWindow", true);

    private void OnEnable()
    {
        _isWindowActive = true;
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
                _nodesList = new ReorderableList(_levelsConfig.LevelsData[list.index].NodesData, typeof(NodeData), 
                    true, true, true, true)
                {
                    drawElementCallback = (rect, index, _, _) =>
                    {
                        rect.y += 2;
                        rect.xMax /= 5;

                        var node = _levelsConfig.LevelsData[list.index].NodesData[index];
                        node.TeamId = _levelsConfig.LevelsData[list.index].NodesData[index].TeamId;
                        node.Position = EditorGUI.Vector3Field(rect, string.Empty, node.Position);

                        rect.x = rect.xMax;
                        node.Radius = EditorGUI.FloatField(rect, "Radius", node.Radius);
                        
                        rect.x = rect.xMax;
                        node.TeamId = EditorGUI.IntField(rect, "Team ID", node.TeamId);

                        rect.x = rect.xMax;
                        node.MaxUnitCount = EditorGUI.IntField(rect, "Max units", node.MaxUnitCount);
                                
                        rect.x = rect.xMax;
                        node.Injection = EditorGUI.IntField(rect, "injection", node.Injection);
                    },
                    drawHeaderCallback = rect => 
                    {  
                        EditorGUI.LabelField(rect, "Nodes");
                    }
                };
                
                LoadLevel(list.index);
            }
        };
        
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        _isWindowActive = false;
    }

    private void OnSceneGUI(SceneView scene)
    {
        if (!_isWindowActive)
            return;
        
        if (Event.current.type == EventType.MouseUp)
        {
            var mousePosition = (Vector3) Event.current.mousePosition;
            var ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            mousePosition = new Vector3(ray.origin.x, ray.origin.y, 0);
            var newPos = mousePosition;
            
            if (_levelsList.index >= 0 && _nodesList.index >= 0)
            {
                _levelsConfig.LevelsData[_levelsList.index].NodesData[_nodesList.index].Position = newPos;
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
                LoadLevel(_levelsList.index);

            if (GUILayout.Button("Save"))
            {
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }
        
        _nodesList?.DoLayoutList();
        
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void LoadLevel(int index)
    {
        this.DestroyObjectsOfType<NodeView>();
        var canvas = FindObjectOfType<Canvas>();
        var lvl = _levelsConfig.LevelsData[index];
        
        foreach (var n in lvl.NodesData)
        {
            var nodeObj = Instantiate(_levelsConfig.NodeView, canvas.transform);
            nodeObj.transform.position = n.Position;
            nodeObj.SetSprite(_levelsConfig.TeamSprites[n.TeamId]);
            nodeObj.SetUnitCountText(n.Injection.ToString());
            nodeObj.SetRadius(n.Radius);
        }
    }
}

#endif