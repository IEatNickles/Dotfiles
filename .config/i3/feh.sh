killall -q feh
while pgrep -x feh >/dev/null; do sleep 1; done

# "$HOME/Pictures/flat.jpeg"
feh --bg-scale "$HOME/Pictures/tag.png"
