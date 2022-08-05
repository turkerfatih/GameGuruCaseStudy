using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Case1
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private Case1Config config;
        [SerializeField] private Transform gridContainer;

        private GridData<Cell> data;
        private int requiredNeighboursCount;
        private int currentSizeX;
        private int currentSizeY;
        private ObjectPool tilePool;
        private ObjectPool itemPool;
        private Bounds bounds;
        private Camera cam;
        private Vector3 offset;
        private Vector3 tileSize;
        private MatchingNeighboursFinder matchingNeighboursFinder;

        private void Awake()
        {
            cam=Camera.main;
            tileSize = Vector3.one * config.TileSize;
            CreatePools();
        }
        private void Start()
        {
            var maxSize = config.MaximumGridSize * config.MaximumGridSize;
            data = new GridData<Cell>(currentSizeX, currentSizeY,maxSize);
            matchingNeighboursFinder = new MatchingNeighboursFinder(requiredNeighboursCount);
            CreateGrid();
        }
        
        public void CreateGrid()
        {

            data.Resize(currentSizeX,currentSizeY);
            tilePool.PutBackAll();
            CreateTiles();
        }

        private void CreateTiles()
        {
            gridContainer.localPosition=Vector3.zero;
            var halfTileSize = config.TileSize / 2f;
            var gridSizeX = currentSizeX * config.TileSize;
            var gridSizeY = currentSizeY * config.TileSize;
            var gridSizeHalfX = gridSizeX/2f;
            var gridSizeHalfY = gridSizeY/2f;
            var gridOffsetX = -gridSizeHalfX + halfTileSize;
            var gridOffsetY = -gridSizeHalfY + halfTileSize;
            offset = new Vector3(gridOffsetX, gridOffsetY);

            for (int width = 0; width < currentSizeX; width++)
            {
                for (int height = 0; height < currentSizeY; height++)
                {
                    var tile=tilePool.GetItem();
                    AddObjectToGrid(tile, width, height);
                 
                }
            }

            bounds = new Bounds
            {
                center = gridContainer.position,
                size = new Vector3( config.TileSize*currentSizeX ,config.TileSize * currentSizeY)
            };

            EventBus.ViewBoundsChange?.Invoke(bounds);
        }

        private void AddObjectToGrid(GameObject subject, int width, int height)
        {
            subject.transform.SetParent(gridContainer);
            subject.transform.localScale = tileSize;
            var pos = new Vector3(tileSize.x * width, tileSize.y * height);
            subject.transform.localPosition = pos + offset;
            subject.SetActive(true);
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                var mousePos = Input.mousePosition;
                var worldPosition = cam.ScreenToWorldPoint(mousePos);
                worldPosition.z = 0;
                var localPoint = gridContainer.InverseTransformPoint(worldPosition);
                if (bounds.Contains(localPoint))
                {
                    var xRatio = Mathf.InverseLerp(bounds.min.x, bounds.max.x, localPoint.x);
                    var yRatio = Mathf.InverseLerp(bounds.min.y, bounds.max.y, localPoint.y);
                    int row = (int)(xRatio * currentSizeX*config.TileSize);
                    int col = (int)(yRatio * currentSizeY*config.TileSize);
                    PutItem(row,col);
                }
            }
            
        }

        private void PutItem(int row,int col)
        {
            var cell = data.GetValue(row, col);
            if (!cell.IsFilled)
            {
                cell.TemporaryFill();
                data.SetValue(row, col, cell);
                if (matchingNeighboursFinder.Search(data, row, col,out HashSet<Vector2Int> result))
                {
                    Debug.Log(result.Count);
                    foreach (var neighbour in result)
                    {
                        RemoveNeighbour(neighbour);
                    }
                }
                else
                {
                    var item = itemPool.GetItem();
                    AddObjectToGrid(item,row,col);
                    cell.Fill(item);
                    data.SetValue(row, col, cell);   
                }

            }
            else
            {
                //todo:Shake existing item
            }
        }

        private void RemoveNeighbour(Vector2Int neighbourIndex)
        {
            var neighbour = data.GetValue(neighbourIndex.x, neighbourIndex.y);
            if (neighbour.Item!=null)
            {
                itemPool.PutItem(neighbour.Item);
            }
            neighbour.Empty();
            data.SetValue(neighbourIndex.x,neighbourIndex.y,neighbour);
        }


        private void CreatePools()
        {
            tilePool = new ObjectPool(config.TilePrefab, config.TilePrefabPoolSize);
            itemPool = new ObjectPool(config.ItemPrefab, config.ItemPrefabPoolSize);
        }
        

        [UsedImplicitly]
        public void OnGridRowCountChange(Single amount)
        {
            currentSizeX = (int)amount;
        }
        
        [UsedImplicitly]
        public void OnGridColCountChange(Single amount)
        {
            currentSizeY = (int)amount;
        }
        
        [UsedImplicitly]
        public void OnMinMatchCountChange(Single amount)
        {
            requiredNeighboursCount = (int)amount;
            if (matchingNeighboursFinder == null)
            {
                matchingNeighboursFinder = new MatchingNeighboursFinder(requiredNeighboursCount);
            }
            else
            {
                matchingNeighboursFinder.SetMinimumRequiredNeighbours(requiredNeighboursCount);
            }
        }
        
        
    }    
}

