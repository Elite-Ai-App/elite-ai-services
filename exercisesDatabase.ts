/**
 * Exercise Database for Elite AI Basketball Workout Program
 */

export interface Exercise {
  category: string;
  name: string;
  level?: string;
  equipment: string;
}

export const strengthExercises = [
  // Lower Body Strength
  {
    category: "Lower Body Strength",
    name: "Calf Raise",
    level: "Beginner",
    equipment: "Dumbbell or Machine",
  },
  {
    category: "Lower Body Strength",
    name: "Seated Calf Raise",
    level: "Intermediate",
    equipment: "Machine",
  },
  {
    category: "Lower Body Strength",
    name: "Standing Calf Raise",
    level: "Intermediate",
    equipment: "Machine or Barbell",
  },
  {
    category: "Lower Body Strength",
    name: "Eccentric Tibialis Kettlebell Raises",
    level: "Intermediate",
    equipment: "Kettlebell",
  },
  {
    category: "Lower Body Strength",
    name: "Back Squat",
    level: "Intermediate",
    equipment: "Barbell, Rack",
  },
  {
    category: "Lower Body Strength",
    name: "Front Squat",
    level: "Intermediate",
    equipment: "Barbell, Rack",
  },
  {
    category: "Lower Body Strength",
    name: "Bulgarian Split Squat",
    level: "Intermediate",
    equipment: "Dumbbells or Barbell",
  },
  {
    category: "Lower Body Strength",
    name: "Romanian Deadlift",
    level: "Intermediate",
    equipment: "Barbell or Dumbbells",
  },
  {
    category: "Lower Body Strength",
    name: "Trap Bar Deadlift",
    level: "Intermediate",
    equipment: "Trap Bar, Weight Plates",
  },
  {
    category: "Lower Body Strength",
    name: "Goblet Squat",
    level: "Beginner",
    equipment: "Dumbbells or Kettlebell",
  },
  {
    category: "Lower Body Strength",
    name: "Step-Up",
    level: "Beginner",
    equipment: "Dumbbells, Bench",
  },
  {
    category: "Lower Body Strength",
    name: "Split Squat",
    level: "Beginner",
    equipment: "Bodyweight or Dumbbells",
  },
  {
    category: "Lower Body Strength",
    name: "Barbell Quarter Squat",
    level: "Advanced",
    equipment: "Barbell",
  },
  {
    category: "Lower Body Strength",
    name: "Single-Leg Romanian Deadlift",
    level: "Intermediate",
    equipment: "Barbell",
  },
  {
    category: "Lower Body Strength",
    name: "Box Squat",
    level: "Intermediate",
    equipment: "Barbell, Box",
  },
  {
    category: "Lower Body Strength",
    name: "Hip Thrust",
    level: "Intermediate",
    equipment: "Barbell, Bench",
  },
  {
    category: "Lower Body Strength",
    name: "Glute Bridge",
    level: "Beginner",
    equipment: "Bodyweight or Barbell",
  },
  {
    category: "Lower Body Strength",
    name: "Nordic Hamstring Curl",
    level: "Advanced",
    equipment: "Bench, Partner or Straps",
  },

  // Upper Body Strength
  {
    category: "Upper Body Strength",
    name: "Dumbbell Bench Press",
    level: "Beginner",
    equipment: "Dumbbells, Bench",
  },
  {
    category: "Upper Body Strength",
    name: "Dumbbell Incline Press",
    level: "Intermediate",
    equipment: "Dumbbells, Incline Bench",
  },
  {
    category: "Upper Body Strength",
    name: "Dumbbell Floor Press",
    level: "Beginner",
    equipment: "Dumbbells",
  },
  {
    category: "Upper Body Strength",
    name: "Dumbbell Shoulder Press",
    level: "Beginner",
    equipment: "Dumbbells",
  },
  {
    category: "Upper Body Strength",
    name: "Dumbbell Lateral Raise",
    level: "Beginner",
    equipment: "Dumbbells",
  },
  {
    category: "Upper Body Strength",
    name: "Dumbbell Single-Arm Tripod Row",
    level: "Intermediate",
    equipment: "Dumbbells, Bench",
  },
  {
    category: "Upper Body Strength",
    name: "Dumbbell Biceps Curl",
    level: "Beginner",
    equipment: "Dumbbells",
  },
  {
    category: "Upper Body Strength",
    name: "Dumbbell Hammer Curl",
    level: "Beginner",
    equipment: "Dumbbells",
  },
  {
    category: "Upper Body Strength",
    name: "Pull ups",
    level: "Intermediate",
    equipment: "Pullup bar",
  },
  {
    category: "Upper Body Strength",
    name: "Inverted Row",
    level: "Beginner",
    equipment: "Barbell",
  },
  {
    category: "Upper Body Strength",
    name: "Cable Tricep Pulldowns",
    level: "Beginner",
    equipment: "Cable",
  },
  {
    category: "Upper Body Strength",
    name: "Bird Dog Row",
    level: "Advanced",
    equipment: "Dumbbell, Bench",
  },

  // Isometric Strength
  {
    category: "Isometric Strength",
    name: "Isometric Split Squat",
    level: "Beginner",
    equipment: "Bodyweight",
  },
  {category: "Isometric Strength", name: "Wall Sit", level: "Beginner", equipment: "Bodyweight"},
  {
    category: "Isometric Strength",
    name: "Isometric Mid-Thigh Pull",
    level: "Intermediate",
    equipment: "Barbell, Rack",
  },
  {
    category: "Isometric Strength",
    name: "Isometric Glute Bridge",
    level: "Beginner",
    equipment: "Bodyweight",
  },
  {
    category: "Isometric Strength",
    name: "Isometric Push-Up Hold",
    level: "Beginner",
    equipment: "Bodyweight",
  },
  {
    category: "Isometric Strength",
    name: "Isometric Calf Raise",
    level: "Beginner",
    equipment: "Bodyweight",
  },
  {
    category: "Isometric Strength",
    name: "Isometric Pull-Up Hold",
    level: "Intermediate",
    equipment: "Pull-Up Bar",
  },
  {
    category: "Isometric Strength",
    name: "Isometric Trap Bar Deadlift Hold",
    level: "Advanced",
    equipment: "Trap Bar, Weight Plates",
  },
  {
    category: "Isometric Strength",
    name: "Isometric Bulgarian Split Squat",
    level: "Intermediate",
    equipment: "Bodyweight",
  },
  {
    category: "Isometric Strength",
    name: "Isometric Shoulder Press Hold",
    level: "Intermediate",
    equipment: "Dumbbells or Barbell",
  },
  {
    category: "Isometric Strength",
    name: "Isometric Hamstring Hold",
    level: "Intermediate",
    equipment: "Bodyweight, Stability Ball",
  },
  {
    category: "Isometric Strength",
    name: "Copenhagen Side Plank",
    level: "Intermediate",
    equipment: "Bench or Elevated Surface",
  },
  {
    category: "Isometric Strength",
    name: "Isometric Knee Extension Hold",
    level: "Intermediate",
    equipment: "Band or Machine",
  },

  // Weighted Isometric Exercises
  {
    category: "Weighted Isometric Exercises",
    name: "Barbell Isometric Split Squat",
    level: "Intermediate",
    equipment: "Barbell",
  },
  {
    category: "Weighted Isometric Exercises",
    name: "Barbell Isometric Calf Raise Hold",
    level: "Intermediate",
    equipment: "Barbell",
  },
  {
    category: "Weighted Isometric Exercises",
    name: "Isometric Shoulder press Hold",
    level: "Intermediate",
    equipment: "Dumbbells",
  },
  {
    category: "Weighted Isometric Exercises",
    name: "Isometric Bicep Curl Hold",
    level: "Beginner",
    equipment: "Dumbbells",
  },

  // Power Reactive Exercises
  {
    category: "Power Reactive Exercises",
    name: "Hang Clean",
    level: "Advanced",
    equipment: "Barbell",
  },
  {
    category: "Power Reactive Exercises",
    name: "Barbell Speed Squats",
    level: "Advanced",
    equipment: "Barbell",
  },
  {
    category: "Power Reactive Exercises",
    name: "Quarter Speed Split Squats",
    level: "Advanced",
    equipment: "Barbell",
  },
  {
    category: "Power Reactive Exercises",
    name: "High Pulls",
    level: "Intermediate",
    equipment: "Barbell",
  },
  {
    category: "Power Reactive Exercises",
    name: "Single Arm Dumbbell Push Press",
    level: "Advanced",
    equipment: "Dumbbell",
  },

  // Core
  {category: "Core", name: "Plank", level: "Beginner", equipment: "Bodyweight"},
  {category: "Core", name: "Side Plank", level: "Beginner", equipment: "Bodyweight"},
  {category: "Core", name: "Dead Bug", level: "Beginner", equipment: "Bodyweight"},
  {
    category: "Core",
    name: "Pallof Press",
    level: "Intermediate",
    equipment: "Cable or Resistance Band",
  },
  {category: "Core", name: "Hanging Leg Raise", level: "Intermediate", equipment: "Pull-Up Bar"},
  {
    category: "Core",
    name: "Hanging Knee Raises",
    level: "Intermediate",
    equipment: "Pull-Up Bar",
  },
  {category: "Core", name: "Ab Wheel Rollout", level: "Intermediate", equipment: "Ab Wheel"},
  {category: "Core", name: "Bird Dog", level: "Beginner", equipment: "Bodyweight"},
  {category: "Core", name: "Hollow Body Hold", level: "Intermediate", equipment: "Bodyweight"},
  {category: "Core", name: "Sit-the-Pot", level: "Intermediate", equipment: "Stability Ball"},
  {
    category: "Core",
    name: "Suitcase Carry",
    level: "Intermediate",
    equipment: "Dumbbell or Kettlebell",
  },
  {category: "Core", name: "Cable Woodchopper", level: "Intermediate", equipment: "Cable"},

  // Lower Body Plyometrics
  {
    category: "Lower Body Plyometrics",
    name: "Box Jumps",
    level: "Beginner",
    equipment: "Bodyweight, Box",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Lateral Bounds",
    level: "Beginner",
    equipment: "Bodyweight",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Tuck Jumps",
    level: "Beginner",
    equipment: "Bodyweight",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Depth Jump",
    level: "Intermediate",
    equipment: "Bodyweight, Box",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Broad Jump to Vertical Jump",
    level: "Beginner",
    equipment: "Bodyweight",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Trap Bar Jumps",
    level: "Advanced",
    equipment: "Trap Bar, Weight Plates",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Dumbbell Jumping Squat",
    level: "Intermediate",
    equipment: "Dumbbells",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Jump Squat",
    level: "Beginner",
    equipment: "Bodyweight",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Skater Hops",
    level: "Beginner",
    equipment: "Bodyweight",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Lateral Pogo Hops",
    level: "Beginner",
    equipment: "Bodyweight",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Pogo Hops",
    level: "Beginner",
    equipment: "Bodyweight",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Single Leg Hopping",
    level: "Intermediate",
    equipment: "Bodyweight",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Split Jump",
    level: "Beginner",
    equipment: "Bodyweight",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Bulgarian Jump Squat",
    level: "Intermediate",
    equipment: "Bench",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Jump Step Up",
    level: "Intermediate",
    equipment: "Bench",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Depth Jump To Hurdle Hop",
    level: "Advanced",
    equipment: "Hurdles",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Dumbbell Split Jump",
    level: "Intermediate",
    equipment: "Bodyweight",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Barbell Jump Squat",
    level: "Advanced",
    equipment: "Barbell",
  },
  {
    category: "Lower Body Plyometrics",
    name: "Single Leg Box Jump",
    level: "Advanced",
    equipment: "Box",
  },

  // Upper Body Plyometrics
  {
    category: "Upper Body Plyometrics",
    name: "Medicine Ball Chest Pass",
    level: "Beginner",
    equipment: "Medicine Ball",
  },
  {
    category: "Upper Body Plyometrics",
    name: "Medicine Ball Step Behind Rotational Throw",
    level: "Intermediate",
    equipment: "Medicine Ball",
  },
  {
    category: "Upper Body Plyometrics",
    name: "Plyo Push-Up",
    level: "Intermediate",
    equipment: "Bodyweight",
  },
  {
    category: "Upper Body Plyometrics",
    name: "Medicine Ball Half Kneeling Chest Push",
    level: "Beginner",
    equipment: "Medicine Ball",
  },
  {
    category: "Upper Body Plyometrics",
    name: "Med Ball Overhead Slam",
    level: "Beginner",
    equipment: "Medicine Ball",
  },
  {
    category: "Upper Body Plyometrics",
    name: "Medicine Ball Rotational Throw",
    level: "Intermediate",
    equipment: "Medicine Ball",
  },
];

export const mobilityExercises = [
  // Hip
  {category: "Hip", name: "Kneeling Hip Flexor Stretch", equipment: "Bodyweight"},
  {category: "Hip", name: "Single Leg Glute Bridge", equipment: "Bodyweight"},
  {category: "Hip", name: "Clamshells with Band", equipment: "Band"},
  {category: "Hip", name: "90/90 Hip Stretch", equipment: "Bodyweight"},
  {category: "Hip", name: "Hip Adductor Rockback", equipment: "Bodyweight"},
  {category: "Hip", name: "Pigeon Stretch", equipment: "Bodyweight"},
  {category: "Hip", name: "World's Greatest Stretch", equipment: "Bodyweight"},
  {category: "Hip", name: "Figure-4 Stretch", equipment: "Bodyweight"},
  {category: "Hip", name: "Lateral Band Walks", equipment: "Band"},
  {category: "Hip", name: "Frog Stretch", equipment: "Bodyweight"},

  // Ankle
  {category: "Ankle", name: "Ankle Wall Dorsiflexion Mobilization", equipment: "Bodyweight"},
  {category: "Ankle", name: "Calf Raise Isometric", equipment: "Bodyweight"},
  {category: "Ankle", name: "Heel Walks", equipment: "Bodyweight"},
  {category: "Ankle", name: "Toe Walks", equipment: "Bodyweight"},
  {category: "Ankle", name: "Ankle Alphabet", equipment: "Bodyweight"},
  {category: "Ankle", name: "Banded Ankle Eversion", equipment: "Band"},
  {category: "Ankle", name: "Single-Leg Balance", equipment: "Bodyweight"},
  {category: "Ankle", name: "Banded Ankle Inversion", equipment: "Band"},
  {category: "Ankle", name: "Tibialis Wall Raise", equipment: "Bodyweight"},
  {category: "Ankle", name: "Bottom of foot rolling (i.e. Golf Ball)", equipment: "Small ball"},

  // Shoulder
  {category: "Shoulder", name: "Scapular Wall Slides", equipment: "Bodyweight"},
  {category: "Shoulder", name: "Face Pulls with Band", equipment: "Band"},
  {category: "Shoulder", name: "Banded/Cable External Rotation", equipment: "Band"},
  {category: "Shoulder", name: "Prone YTWL Shoulder Drill", equipment: "Bodyweight"},
  {category: "Shoulder", name: "Shoulder Dislocates", equipment: "Band, Bar"},
  {category: "Shoulder", name: "Arm Circles", equipment: "Bodyweight"},
  {category: "Shoulder", name: "Scapula Push Ups", equipment: "Bodyweight"},
  {category: "Shoulder", name: "Bar Hangs", equipment: "Pull Up Bar"},
  {category: "Shoulder", name: "Overhead Reach with Band", equipment: "Band"},
  {category: "Shoulder", name: "Serratus Wall Slide", equipment: "Bodyweight"},

  // Thoracic Spine
  {category: "Thoracic Spine", name: "Cat-Cow Stretch", equipment: "Bodyweight"},
  {category: "Thoracic Spine", name: "Thread the Needle", equipment: "Bodyweight"},
  {category: "Thoracic Spine", name: "Quadruped T-Spine Rotation", equipment: "Bodyweight"},
  {category: "Thoracic Spine", name: "Foam Roller Thoracic Extension", equipment: "Foam Roller"},
  {category: "Thoracic Spine", name: "Spine Lumbar Twist Stretch", equipment: "Bodyweight"},
  {category: "Thoracic Spine", name: "Wall Angels", equipment: "Bodyweight"},
  {category: "Thoracic Spine", name: "Kneeling Lat Stretch", equipment: "Bodyweight"},
  {category: "Thoracic Spine", name: "Child's Pose with Side Reach", equipment: "Bodyweight"},
  {category: "Thoracic Spine", name: "Thoracic Bridge", equipment: "Bodyweight"},

  // Elbow
  {category: "Elbow", name: "Wrist Curls with Band", equipment: "Band"},
  {category: "Elbow", name: "Reverse Wrist Curls with Band", equipment: "Band"},
  {category: "Elbow", name: "Banded Triceps Extension", equipment: "Band"},
  {category: "Elbow", name: "Forearm Pronation/Supination with Band", equipment: "Band"},
  {category: "Elbow", name: "Eccentric Wrist Flexion", equipment: "Band"},

  // Knee
  {category: "Knee", name: "Terminal Knee Extension with Band", equipment: "Band"},
  {category: "Knee", name: "Peterson Step-Up", equipment: "Bodyweight"},
  {category: "Knee", name: "Lateral Step-Down", equipment: "Bodyweight"},
  {category: "Knee", name: "Wall Sit", equipment: "Bodyweight"},
  {category: "Knee", name: "Hamstring Bridge", equipment: "Bodyweight"},
  {category: "Knee", name: "Spanish Squat with Band", equipment: "Band"},
  {category: "Knee", name: "Single Leg Wall Sit", equipment: "Bodyweight"},
  {category: "Knee", name: "Reverse Nordic Curl", equipment: "Bodyweight"},
  {category: "Knee", name: "Single-Leg Squat to Box", equipment: "Bodyweight"},

  // Wrists
  {category: "Wrists", name: "Wrist Flexor Stretch", equipment: "Bodyweight"},
  {category: "Wrists", name: "Wrist Extensor Stretch", equipment: "Bodyweight"},
  {category: "Wrists", name: "Reverse Curls with Band", equipment: "Band"},
  {category: "Wrists", name: "Wrist Curls with Band", equipment: "Band"},
  {category: "Wrists", name: "Wrist Circles", equipment: "Bodyweight"},

  // Lower Back
  {category: "Lower Back", name: "Side Plank", equipment: "Bodyweight"},
  {category: "Lower Back", name: "Roll Ball Low Back", equipment: "Bodyweight"},
  {category: "Lower Back", name: "Bird Dogs", equipment: "Bodyweight"},
  {category: "Lower Back", name: "Cat-Cow", equipment: "Bodyweight"},
  {category: "Lower Back", name: "Superman Exercise", equipment: "Bodyweight"},
  {category: "Lower Back", name: "Dead Bug", equipment: "Bodyweight"},
  {category: "Lower Back", name: "Glute Bridge", equipment: "Bodyweight"},
  {category: "Lower Back", name: "Reverse Hyperextensions", equipment: "Bodyweight"},
  {category: "Lower Back", name: "Bodyweight Romanian Deadlifts", equipment: "Bodyweight"},
  {category: "Lower Back", name: "Jefferson Curl with Band", equipment: "Bodyweight"},
  {category: "Lower Back", name: "Pelvic Tilts", equipment: "Bodyweight"},

  // Hamstring
  {category: "Hamstring", name: "Single Leg Romanian Deadlift", equipment: "Bodyweight"},
  {category: "Hamstring", name: "Isometric Hamstring bridge hold", equipment: "Bodyweight"},
  {category: "Hamstring", name: "Supine Hamstring Stretch", equipment: "Bodyweight"},
  {category: "Hamstring", name: "Glute Bridge Walk", equipment: "Bodyweight"},

  // Shin Splints
  {category: "Shin Splints", name: "Tibialis Wall Raises", equipment: "Bodyweight"},
  {category: "Shin Splints", name: "Towel Toe Scrunches", equipment: "Bodyweight"},
  {category: "Shin Splints", name: "Foam Rolling Calves", equipment: "Bodyweight"},
  {category: "Shin Splints", name: "Kettlebell Tibialis raises", equipment: "Kettlebell"},

  // Groin
  {category: "Groin", name: "Butterfly stretch", equipment: "Bodyweight"},
  {category: "Groin", name: "Copenhagen Side Plank", equipment: "Bench"},
  {category: "Groin", name: "Roll ball Adductor", equipment: "Small Ball"},
  {category: "Groin", name: "Adductor Stretch against wall", equipment: "Bodyweight"},
];
