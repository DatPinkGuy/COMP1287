﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EndZone : MonoBehaviour
{
    public bool GameEnd => _gameEnd;
    private bool _gameEnd;
    private int _aliveAgents;
    private float Timer => _watchScript.Timer;
    private Watch _watchScript;
    private BuildingAndMovementScript _mainScript;
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    [SerializeField] private int requiredAgents;
    [SerializeField] private float[] completionTimes;
    [SerializeField] private int[] rewardLevels;
    [SerializeField] private Image[] starsForTime;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text text;
    [SerializeField] private String gameWon;
    [SerializeField] private String gameLost;
    [SerializeField] private AudioClip[] winLoseSound;
    [HideInInspector] public List<AgentCharacters> agents;
    [HideInInspector] public List<AgentCharacters> agentsInside;

    // Start is called before the first frame update
    void Start()
    {
        _mainScript = FindObjectOfType<BuildingAndMovementScript>();
        _watchScript = FindObjectOfType<Watch>();
        _particleSystem = canvas.GetComponent<ParticleSystem>();
        _audioSource = canvas.GetComponent<AudioSource>();
        agents.AddRange(FindObjectsOfType<AgentCharacters>());
        foreach (var image in starsForTime)
        {
            image.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameEnd) return;
        RotateCanvas();
        CheckAliveAgents();
        CheckAgentsInside();
        if (_aliveAgents != 0) return;
        _gameEnd = true;
        _mainScript.GameActive = false;
        text.text = gameLost;
        _audioSource.clip = winLoseSound[0];
        _audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var agent in agents)
        {
            if (other.gameObject != agent.gameObject) continue;
            agentsInside.Add(agent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var agent in agentsInside)
        {
            if (other.gameObject != agent.gameObject) continue;
            agentsInside.Remove(agent);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_mainScript.cycle == BuildingAndMovementScript.Cycle.Night) return;
        foreach (var agent in agentsInside)
        {
            agent.health += agent.healthUsage * Time.deltaTime;
        }
    }

    private void CheckAliveAgents()
    {
        _aliveAgents = 0;
        foreach (var agent in agents)
        {
            if (agent.isActiveAndEnabled) _aliveAgents++;
        }
    }

    private void CheckAgentsInside()
    {
        if (_aliveAgents == agentsInside.Count)
        {
            if (_aliveAgents >= requiredAgents)
            {
                _gameEnd = true;
                _mainScript.GameActive = false;
                text.text = gameWon;
                _particleSystem.Play();
                _audioSource.clip = winLoseSound[1];
                _audioSource.Play();
                StartCoroutine(GameWon());
            }
            else
            {
                _gameEnd = true;
                _mainScript.GameActive = false;
                text.text = gameLost;
                _audioSource.clip = winLoseSound[0];
                _audioSource.Play();
            }
        }
    }

    private void RotateCanvas()
    {
        Vector3 direction = _mainScript.centerCamera.transform.position - canvas.transform.position;
        canvas.transform.rotation = Quaternion.LookRotation(direction);
    }
    
    IEnumerator GameWon()
    {
        if (Timer >= completionTimes.Last())
        {
            _watchScript.currency += rewardLevels.Last();
            starsForTime.First().enabled = true;
        }
        else if (Timer <= completionTimes.First())
        {
            _watchScript.currency += rewardLevels.First();
            foreach (var image in starsForTime)
            {
                image.enabled = true;
            }
        }
        else
        {
            _watchScript.currency += rewardLevels[rewardLevels.Length / 2];
            for (int i = 0; i < starsForTime.Length-1; i++)
            {
                starsForTime[i].enabled = true;
            }
        }
        _watchScript.UpdateCurrency();
        _watchScript.LevelStartCurrency = _watchScript.currency;
        SaveSystem.SaveCurrency(_watchScript);
        enabled = false;
        yield return null;
    }

}
