/*

#include "pattrigue_danish.c"

Add the following to the very start of process_record_user before the switch case in keymap.c:

if (!process_record_user_danish(keycode, record)) return false;

Add the following function call:

void matrix_scan_user(void) {
	 check_danish_mod_tap_timers(); 
}

And:
make moonlander:layout
*/

#define NUM_DANISH_MOD_TAPS 3
#define TOGGLE_DANISH_MOD_TAP_KEYCODE KC_F24

float toggle_danish_mod_tap_on_song[][2] = SONG(PLANCK_SOUND);
float toggle_danish_mod_tap_off_song[][2] = SONG(MUSIC_OFF_SOUND);

bool danish_mod_tap_enabled = true;

typedef struct {
    uint16_t timer;
    uint16_t keycode;
    bool     held;
} danish_mod_tap;

danish_mod_tap danish_mod_taps[NUM_DANISH_MOD_TAPS] = 
{
	{.keycode = DK_ARNG},
	{.keycode = DK_AE},
	{.keycode = DK_OSTR}
};

void toggle_danish_mod_tap(void) {
    danish_mod_tap_enabled = !danish_mod_tap_enabled;

#ifdef AUDIO_ENABLE
    if (danish_mod_tap_enabled) {
        PLAY_SONG(toggle_danish_mod_tap_on_song);
    } else {
        PLAY_SONG(toggle_danish_mod_tap_off_song);
    }
#endif
}

bool danish_mod_tap_key(uint16_t keycode, keyrecord_t *record, uint16_t i) {
    if (record->event.pressed) {
        danish_mod_taps[i].timer = timer_read();
        danish_mod_taps[i].held = true;
        tap_code16(keycode);
    } else {
        danish_mod_taps[i].held = false;
        danish_mod_taps[i].timer = 0;
    }
    return false;
}

void reset_danish_mod_taps(void) {
	for (uint16_t i = 0; i < NUM_DANISH_MOD_TAPS; i++) {
		danish_mod_taps[i].timer = 0;
        danish_mod_taps[i].held = false;
    }
}

bool process_record_user_danish(uint16_t keycode, keyrecord_t *record) {
    if (keycode == TOGGLE_DANISH_MOD_TAP_KEYCODE && record->event.pressed) {
        toggle_danish_mod_tap();    
    }

    if (!danish_mod_tap_enabled) return true;

    if (record->event.pressed) {
		reset_danish_mod_taps();
	}

    switch (keycode) {
		case KC_A:
			return danish_mod_tap_key(keycode, record, 0);
		case KC_E:
			return danish_mod_tap_key(keycode, record, 1);
		case KC_O:
			return danish_mod_tap_key(keycode, record, 2);
    }

    return true;
}

void check_danish_mod_tap_timers(void) {
    if (!danish_mod_tap_enabled) return;

    for (uint16_t i = 0; i < NUM_DANISH_MOD_TAPS; i++) {
        if (danish_mod_taps[i].held) {
            if (timer_elapsed(danish_mod_taps[i].timer) > TAPPING_TERM) {
                tap_code16(KC_BSPACE);
                tap_code16(danish_mod_taps[i].keycode);

       			danish_mod_taps[i].timer = 0;
                danish_mod_taps[i].held = false;
            }
        }
    }
}