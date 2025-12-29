#!/bin/bash

hour=$(date +"%H")
if [ $hour -ge 18 ] || [ $hour -le 8 ]; then
  feh --bg-scale --randomize $HOME/Pictures/wallpapers/night/*
else
  feh --bg-scale --randomize $HOME/Pictures/wallpapers/day/*
fi
