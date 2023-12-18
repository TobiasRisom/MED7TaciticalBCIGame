import pandas as pd
import os
import statistics


# Read the CSV file into a DataFrame
def read_csv_to_dataframe(file_path):
    try:
        df = pd.read_csv(file_path, sep=';')
        return df
    except FileNotFoundError:
        print(f"Error: File not found at path {file_path}")
        return None
    except Exception as e:
        print(f"An error occurred: {e}")
        return None


# Accesses the folders to get the Game and Sample files per participant and version
# x is the number of participants + 1
def get_folder(x):
    # Defining the base directory where your participant folders are located
    base_directory = "Data"

    # Looping through participant folders
    for participant_number in range(1, x):
        # os.path.join merges the two paths to create one path to the individual participant folder.
        participant_folder = os.path.join(base_directory, f"Participant {participant_number}")

        # Looping through Battery and Interval folders
        for subfolder in ["Battery", "Interval"]:
            # Joins the prior path with either Battery or Interval
            subfolder_path = os.path.join(participant_folder, subfolder)

            # Creates a variable that changes depending on which version
            if subfolder == "Battery":
                type_fold = 0
            elif subfolder == "Interval":
                type_fold = 1

            # Finds all files in the subfolder
            all_files = os.listdir(subfolder_path)
            # Filter files that ends with the common file name Game or Sample and saves those
            relevant_files = [file for file in all_files if file.endswith("Game.csv") or file.endswith("Sample.csv")]
            # Looping through relevant files
            for file in relevant_files:
                # Creates the final path to the file
                file_path = os.path.join(subfolder_path, file)
                # Check if the file ends with "game" or "sample" and puts it in a dataframe
                if file.endswith("Game.csv"):
                    data_frame = read_csv_to_dataframe(file_path)
                    print('Game file loaded')
                elif file.endswith("Sample.csv"):
                    data_frame2 = read_csv_to_dataframe(file_path)
                    print('Sample file loaded')
                else:
                    print(f"Unrecognized file type: {file_path}")
            # Calls the count_events() method
            results = count_events(data_frame, type_fold, data_frame2)
            results.to_csv('Data/Outputs_all/Output_' + str(participant_number) + '_' + str(type_fold))


# Gets the 8 last samples of confidence before a success or fail MI attempt
def grab_buffer(df, frame):
    # Compares the framecount from the Game file to the Sample file, finds the index of the one closest to it.
    # #Always picks the lower number in Samples
    closest_index = (df['Framecount'] - frame < 0).abs().idxmin()

    # Check if any index was found
    if closest_index:
        index_value = closest_index
        start_index = max(0, index_value - 15)  # Ensure not to go below index 0, and grabs the 8 last values
        end_index = index_value - 1
        confidences = df.loc[start_index:end_index, 'BCIConfidence'].values   # Locates the confidences of the 8 indexes
    print(confidences)
    miny = min(confidences)
    print('min' + str(miny))
    maxy = max(confidences)
    print('max' + str(maxy))
    median = statistics.median(confidences)
    print('med' + str(median))
    sd = statistics.stdev(confidences)
    print('sd' + str(sd))

    return miny, maxy, median, sd



def count_events(data_framer, typer, df2):
    # Created a dataframe to put the event data into
    output = pd.DataFrame(columns=['Index', 'Frame', 'Success', 'Goblin Death', 'Player Hurt',
                                   'Player Attacking', 'Player Moving', 'Min', 'Max', 'Median', 'Sd', 'Type'])

    # Counters for each event type
    goblin_death_count = 0
    player_hurt_count = 0
    player_attacking_count = 0
    player_moving_count = 0
    row_num = 0

    # Iterate over the 'Event' column, adding to the counter for each event type
    for index, column in data_framer.iterrows():
        event = column['Event']
        if event == 'Goblin Death':
            goblin_death_count += 1
        elif event == 'Player Hurt':
            player_hurt_count += 1
        elif event == 'Player Attacking':
            player_attacking_count += 1
        elif event == 'Player Moving':
            player_moving_count += 1
        elif event in ['BciSuccess', 'BciFail']:
            # Adding a new row to the DataFrame for BciSuccess or BciFail
            success_value = 1 if event == 'BciSuccess' else 0
            frames = data_framer['Framecount'][index]   # Gets the framecount for the current index
            miny, maxy, median, sd = grab_buffer(df2, frames)  # Gets the 8 last confidences from the Sample file based on the framecount

            # Puts all the event data into a new row, then concatenates with the output dataframe
            new_row = {'Index': index, 'Frame': frames, 'Success': success_value, 'Goblin Death': goblin_death_count,
                       'Player Hurt': player_hurt_count, 'Player Attacking': player_attacking_count,
                       'Player Moving': player_moving_count, 'Min': miny, 'Max': maxy, 'Median': median, 'Sd': sd, 'Type': typer}
            # Convert the new row to a DataFrame
            new_row_df = pd.DataFrame([new_row], columns=output.columns)

            # Concatenate the new row DataFrame to the output DataFrame
            output = pd.concat([output, new_row_df], ignore_index=True)
            print(f"Processed row {index}, output shape: {output.shape}")
    return output


# Analyses all files for all 14 participants.
get_folder(15)
