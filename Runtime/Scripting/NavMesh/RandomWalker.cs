using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PowderBox.Extensions.CollectionExtensions;

namespace PowderBox
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class RandomWalker : MonoBehaviour
    {
        [SerializeField] GameObject _destinationsParent;
        [SerializeField] float _destinationPrecision = 0.1f;

        Transform[] _possibleDestinations;
        NavMeshAgent _agent;
        bool _isWalking;

        public Action onDestinationReached;
        public Action onMovementStarted;

        public void SetNewDestination()
        {
            Vector3 destination = _possibleDestinations.RandomElement().position;
            _agent.SetDestination(destination);
            onMovementStarted?.Invoke();
            _isWalking = true;
        }

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _possibleDestinations = _destinationsParent.transform.GetComponentsInChildren<Transform>();
        }

        void Update()
        {
            if (_agent.remainingDistance < _destinationPrecision && _isWalking)
            {
                onDestinationReached?.Invoke();
                _isWalking = false;
            }
        }
    }
}
