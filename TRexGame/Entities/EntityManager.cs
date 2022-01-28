using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;

namespace TRexGame.Entities
{
    public class EntityManager
    {
        private readonly List<IGameEntity> _entities = new List<IGameEntity>();
        private readonly List<IGameEntity> _entitiesToAdd = new List<IGameEntity>();
        private readonly List<IGameEntity> _entitiesToRemove = new List<IGameEntity>();

        // breaks incapsulation, since user can use this property to access _entities list, cast it to "List" type. "IEnumerable" doesn't 
        // expose any methods to manipulate the list. So we want to return only readonly version of this list
        public IEnumerable<IGameEntity> Entities => new ReadOnlyCollection<IGameEntity>(_entities);

        public void Update(GameTime gameTime)
        {
            // we want to update all the entities
            foreach(IGameEntity entity in _entities)
            {
                // However, we don't want to update entities that we are in the 'remove' list
                if (_entitiesToRemove.Contains(entity))
                    continue;

                entity.Update(gameTime);
            }

            // update the _entity list with newly added entities from "AddEntities" method
            foreach(IGameEntity entity in _entitiesToAdd)
            {
                _entities.Add(entity);
            }

            // remove the entities from _entity list
            foreach (IGameEntity entity in _entitiesToRemove)
            {
                _entities.Remove(entity);
            }


            // So that we can use this to add new entities to main list later withouth adding duplicates from before
            _entitiesToAdd.Clear();
            _entitiesToRemove.Clear();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // sort entities in ascending order by drawOrder property. first priority is 0 and then increasing
            foreach(IGameEntity entity in _entities.OrderBy(e => e.DrawOrder))
            {
                entity.Draw(spriteBatch, gameTime);
            }

        }

        public void AddEntity(IGameEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity), "Null cannot be added as entity.");

            // you should not update a collection that you are iterating over.
            // instead create new list and update the old one as soon as "Update" method is done
            _entitiesToAdd.Add(entity);
        }

        public void RemoveEntity(IGameEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity), "Null is not a valid entity that can be removed.");

            _entitiesToRemove.Add(entity);
        }

        public void Clear()
        {
            // all current existing entities are added to "_entitiesToRemove" and at the next update all entities will be removed
            _entitiesToRemove.AddRange(_entities); 
        }

        // you can pass ex. Trex and this method will return only TRex entities
        // this.GetEntitiesOfType<TRex>();
        public IEnumerable<T> GetEntitiesOfType<T>() where T : IGameEntity
        {
            return _entities.OfType<T>();
            
        }
    }
}
