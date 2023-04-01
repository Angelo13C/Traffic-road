using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class FootstepAudioPlayerSystem : SystemBase
{
    private const float MIN_VELOCITY_FOR_FOOTSTEP_SOUND = 0.5f;
    
    protected override void OnUpdate()
    {
        foreach (var (footstepAudioPlayer, characterBody, transform) in SystemAPI.Query<FootstepAudioPlayer, KinematicCharacterBody, LocalTransform>())
        {
            if(!characterBody.IsGrounded || math.lengthsq(characterBody.RelativeVelocity) < MIN_VELOCITY_FOR_FOOTSTEP_SOUND * MIN_VELOCITY_FOR_FOOTSTEP_SOUND)
                footstepAudioPlayer.AudioPlayer.SetPlayerFootstepSurface(AudioPlayer.FootstepSurface.None);
            else
            {
                if (SystemAPI.TryGetSingletonBuffer<MapTile>(out var mapTiles))
                {
                    var tileIndex = (int) math.round(transform.Position.z / MapTilePrefab.TILE_WIDTH);
                    var walkingSurface = AudioPlayer.FootstepSurface.None;
                    if (tileIndex < 0)
                        walkingSurface = AudioPlayer.FootstepSurface.Grass;
                    else
                    {
                        var walkingOnTile = mapTiles[tileIndex].Tile;
                        if(walkingOnTile == TileType.Grass)
                            walkingSurface = AudioPlayer.FootstepSurface.Grass;
                        else if(walkingOnTile == TileType.Road)
                            walkingSurface = AudioPlayer.FootstepSurface.Road;
                    }
                    
                    var footPosition = transform.Position;
                    footPosition.y += footstepAudioPlayer.FootYOffset;
                    footstepAudioPlayer.AudioPlayer.SetPlayerFootstepSurface(walkingSurface, footPosition);
                }
            }
        }
    }
}